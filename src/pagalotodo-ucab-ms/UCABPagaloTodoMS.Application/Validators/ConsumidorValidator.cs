using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Text.RegularExpressions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Validators
{
    /// <summary>
    /// Clase que define las reglas de validación para la creación de un consumidor.
    /// </summary>
    public class ConsumidorValidator : AbstractValidator<ConsumidorRequest>
    {
        private readonly IUCABPagaloTodoDbContext _dbcontext; 

        /// <summary>
        /// Constructor de la clase ConsumidorValidator.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos para acceder a la información del consumidor.</param>
        public ConsumidorValidator(IUCABPagaloTodoDbContext dbContext)
            {
            _dbcontext = dbContext;

                RuleFor(x => x.NombreUsuario)
                    .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío.")
                    .NotNull().WithMessage("El nombre de usuario es obligatorio.");


                RuleFor(x => x.NombreUsuario)
                      .MustAsync(async (_nombreusuario, cancellation) =>
                      {
                          var usuarioExistente = await _dbcontext.Consumidores.AnyAsync(c => c.NombreUsuario == _nombreusuario);
                          return !usuarioExistente;
                      }).WithMessage("El Nombre de Usuario ya existe en la base de datos.");
           
                RuleFor(x => x.Cedula)
                    .NotEmpty().WithMessage("La cedula del usuario no puede estar vacío.")
                    .NotNull().WithMessage("El cedula del usuario es obligatorio.");

                RuleFor(x => x.Cedula)
                    .MustAsync(async (cedula, cancellation) =>
                    {
                        var cedulaExistente = await _dbcontext.Consumidores.AnyAsync(c => c.Cedula == cedula);
                        return !cedulaExistente;
                    }).WithMessage("La cedula ya está registrada en la base de datos.");

                RuleFor(x => x.Clave)
                        .NotEmpty().WithMessage("La clave no puede estar vacía.")
                        .NotNull().WithMessage("La clave es obligatoria.")
                        .MinimumLength(8).WithMessage("La clave debe tener al menos 8 caracteres.");

                RuleFor(x => x.Correo)
                    .NotEmpty().WithMessage("El correo no puede estar vacío.")
                    .NotNull().WithMessage("El correo es obligatorio.")
                    .Matches(new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")).WithMessage("El correo debe ser válido.");

                RuleFor(x => x.Correo)
                    .MustAsync(async (correo, cancellation) =>
                    {
                        var correoExistente = await _dbcontext.Consumidores.AnyAsync(c => c.Correo == correo);
                        return !correoExistente;
                    }).WithMessage("El correo ya existe en la base de datos.");

                RuleFor(x => x.Nombre)
                    .NotEmpty().WithMessage("El nombre no puede estar vacío.")
                    .NotNull().WithMessage("El nombre es obligatorio.");

                RuleFor(x => x.Apellido)
                    .NotEmpty().WithMessage("El apellido no puede estar vacío.")
                    .NotNull().WithMessage("El apellido es obligatorio.");

                RuleFor(x => x.Telefono)
                    .NotEmpty().WithMessage("El teléfono no puede estar vacío.")
                    .NotNull().WithMessage("El teléfono es obligatorio.")
                    .GreaterThan(0).WithMessage("El teléfono debe ser mayor que 0.");

                RuleFor(x => x.Direccion)
                    .MaximumLength(100).WithMessage("La dirección debe tener una longitud máxima de 100 caracteres.");
            }
        
    }
}
