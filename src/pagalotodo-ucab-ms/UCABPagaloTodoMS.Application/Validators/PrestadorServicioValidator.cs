using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Text.RegularExpressions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace UCABPagaloTodoMS.Application.Validators
{
    /// <summary>
    /// Clase que define las reglas de validación para la creación de un prestador de servicio.
    /// </summary>
    public class PrestadorServicioValidator : AbstractValidator<PrestadorServicioRequest>
    {
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva

        /// <summary>
        /// Constructor de la clase PrestadorServicioValidator.
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos.</param>
        public PrestadorServicioValidator(IUCABPagaloTodoDbContext dbContext)
        {
            _dbcontext = dbContext;

            RuleFor(x => x.NombreUsuario)
                .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío.")
                .NotNull().WithMessage("El nombre de usuario es obligatorio.");

            RuleFor(x => x.Clave)
                .NotEmpty().WithMessage("La clave no puede estar vacía.")
                .NotNull().WithMessage("La clave es obligatoria.")
                .MinimumLength(8).WithMessage("La clave debe tener al menos 8 caracteres.");

            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El correo no puede estar vacío.")
                .NotNull().WithMessage("El correo es obligatorio.")
                .Matches(new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")).WithMessage("El correo debe ser válido.");

            RuleFor(x => x.NombreEmpresa)
                 .NotEmpty().WithMessage("El nombre de la empresa no puede estar vacío.")
                 .NotNull().WithMessage("El nombre de la empresa es obligatorio.");

            RuleFor(x => x.Rif)
                .NotEmpty().WithMessage("El RIF no puede estar vacío.")
                .NotNull().WithMessage("El RIF es obligatorio.")
                .Matches(new Regex(@"^[VEJPG][0-9]{9}$")).WithMessage("El RIF debe ser válido. ejemplo J123456789");
        }
    }
}