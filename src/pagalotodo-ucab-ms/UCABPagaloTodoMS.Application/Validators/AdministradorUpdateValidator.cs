using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Text.RegularExpressions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace UCABPagaloTodoMS.Application.Validators
{
    /// <summary>
    /// Validador para la actualización de un administrador.
    /// </summary>
    public class AdministradorUpdateValidator : AbstractValidator<AdministradorUpdateRequest>
    {
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva

        /// <summary>
        /// Crea una nueva instancia del validador de actualización de administrador.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos.</param>
        public AdministradorUpdateValidator(IUCABPagaloTodoDbContext dbContext)
            {
                 _dbcontext = dbContext;

                RuleFor(x => x.NombreUsuario)
                    .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío.")
                    .NotNull().WithMessage("El nombre de usuario es obligatorio.");

                //RuleFor(x => x.NombreUsuario)
                //  .MustAsync(async (_nombreusuario, cancellation) =>
                //  {
                //      var usuarioExistente = await _dbcontext.Administradores.AnyAsync(c => c.NombreUsuario == _nombreusuario);
                //      return !usuarioExistente;
                //  }).WithMessage("El Nombre de Usuario ya existe en la base de datos.");

                RuleFor(x => x.Clave)
                    .NotEmpty().WithMessage("La clave no puede estar vacía.")
                    .NotNull().WithMessage("La clave es obligatoria.")
                    .MinimumLength(8).WithMessage("La clave debe tener al menos 8 caracteres.");

                RuleFor(x => x.Correo)
                    .NotEmpty().WithMessage("El correo no puede estar vacío.")
                    .NotNull().WithMessage("El correo es obligatorio.")
                    .Matches(new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")).WithMessage("El correo debe ser válido.");

                //RuleFor(x => x.Correo)
                //       .MustAsync(async (correo, cancellation) =>
                //       {
                //           var correoExistente = await _dbcontext.Consumidores.AnyAsync(c => c.Correo == correo);
                //           return !correoExistente;
                //       }).WithMessage("El correo ya existe en la base de datos.");
                
                RuleFor(x => x.NombreAdministrador)
                        .NotEmpty().WithMessage("El nombre no puede estar vacío.")
                        .NotNull().WithMessage("El nombre es obligatorio.");
                      
            }
        
    }
}
