using FluentAssertions.Execution;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OtpSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Text;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Services;
using UCABPagaloTodoMS.Core.Database;
using Wiry.Base32;


namespace UCABPagaloTodoMS.Application.Handlers;

[ExcludeFromCodeCoverage]
public class RecuperarContrasenaCommandHandler : IRequestHandler<RecuperaContrasenaCommand, string>
{
    private readonly IMailService _mailService;
    private readonly IUCABPagaloTodoDbContext _dbContext;

    public RecuperarContrasenaCommandHandler(IMailService mailService, IUCABPagaloTodoDbContext usuarioRepository)
    {
        _mailService = mailService;
        _dbContext = usuarioRepository;
    }

    public async Task<string> Handle(RecuperaContrasenaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.Usuario.Correo);

            if (usuario == null)
            {
                throw new Exception("El correo electrónico ingresado no existe.");
            }

            // Genera un código temporal de 6 dígitos con una duración de validez de 5 minutos
            var totp = new Totp(Encoding.UTF8.GetBytes(GenerarNumeroAleatorioDe6Digitos().ToString()),step: 300);
            var codigo = totp.ComputeTotp(DateTime.UtcNow);

            var asuntoMensaje = "Código de recuperación de contraseña";
            var cuerpoMensaje = $"Su código de recuperación de contraseña es: {codigo}. Este código es válido por 5 minutos.";
            
            // Envía el correo electrónico con el código y el enlace a la página de cambiarContraseña
            await _mailService.EnviarCorreoElectronicoAsync(request.Usuario.Correo, asuntoMensaje, cuerpoMensaje);

            return codigo;
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"Excepción capturada: {ex.Message}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"Excepción interna: {ex.InnerException.Message}");
            }

            Console.WriteLine($"Código de estado de respuesta: {ex.StatusCode}");
            Console.WriteLine($"Categoría de código de estado de respuesta: {ex.StatusCode}");
            Console.WriteLine($"Respuesta del servidor: {ex.ToString()}");

            return null;
        }
    }

    /// <summary>
    /// Genera un número aleatorio de 6 dígitos.
    /// </summary>
    /// <returns>El número aleatorio generado.</returns>
    private static int GenerarNumeroAleatorioDe6Digitos()
    {
        var random = new Random();
        return random.Next(100000, 999999);
    }
}