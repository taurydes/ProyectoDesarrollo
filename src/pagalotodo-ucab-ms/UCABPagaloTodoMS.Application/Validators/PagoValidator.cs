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
     /// Clase que define las reglas de validación para la creación de un pago.
     /// </summary>
    public class PagoValidator : AbstractValidator<CrearPagoRequest>
    {
        /// <summary>
        /// Constructor de la clase PagoValidator.
        /// </summary>
        public PagoValidator()
        {
           
            RuleFor(x => x.ServicioPrestadoId)
                .NotNull().WithMessage("El Id del servicio prestado es obligatorio.");

            RuleFor(x => x.ConsumidorId)
                .NotNull().WithMessage("El Id del consumidor es obligatorio.");

            RuleFor(x => x.Referencia)
                .NotEmpty().WithMessage("La referencia no puede estar vacía.")
                .NotNull().WithMessage("La referencia es obligatoria.");

        }
    }
}
