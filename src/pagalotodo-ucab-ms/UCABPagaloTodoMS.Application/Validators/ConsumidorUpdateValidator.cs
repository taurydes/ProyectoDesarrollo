using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Text.RegularExpressions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Validators
{
    /// <summary>
    /// Validador para actualizar un consumidor.
    /// </summary>
    public class ConsumidorUpdateValidator : AbstractValidator<ConsumidorUpdateRequest>
    {
        /// <summary>
        /// Crea una nueva instancia del validador de actualización de consumidor.
        /// </summary>
        public ConsumidorUpdateValidator()
        {
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