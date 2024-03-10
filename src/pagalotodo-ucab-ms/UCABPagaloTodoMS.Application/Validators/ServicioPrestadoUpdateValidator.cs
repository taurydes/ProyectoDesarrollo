using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{   
     /// <summary>
     /// Clase que define las reglas de validación para la actualización de un servicio prestado.
     /// </summary>
    public class ServicioPrestadoUpdateValidator : AbstractValidator<ServicioPrestadoUpdateRequest>
    {
        /// <summary>
        /// Constructor de la clase ServicioPrestadoUpdateValidator.
        /// </summary>
        public ServicioPrestadoUpdateValidator()
        {
            
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del servicio no puede estar vacío.")
                .NotNull().WithMessage("El nombre del servicio es obligatorio.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción del servicio no puede estar vacía.")
                .NotNull().WithMessage("La descripción del servicio es obligatoria.");

            RuleFor(x => x.Costo)
                .NotNull().WithMessage("El costo del servicio es obligatorio.")
                .GreaterThan(0).WithMessage("El costo del servicio debe ser mayor a cero.");

            RuleFor(x => x.EstadoServicio)
                .NotEmpty().WithMessage("El estado del servicio no puede estar vacío.")
                .NotNull().WithMessage("El estado del servicio es obligatorio.")
                .Must(x => x == "activo" || x == "inactivo" || x == "pronto")
                .WithMessage("El estado del servicio debe ser 'activo', 'inactivo' o 'pronto'.");

            RuleFor(x => x.TipoPago)
                .NotNull().WithMessage("El tipo de pago es obligatorio.");
        }
    }
}
