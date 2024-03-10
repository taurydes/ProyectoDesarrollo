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
    /// Validador para la creación de un campo de pago, es decir asociado a un pago.
    /// </summary>
    public class CampoPagoValidator : AbstractValidator<CrearCampoPagoRequest>
    {
        public CampoPagoValidator()
        {

            RuleFor(x => x.NombreCampo)
                .NotEmpty().WithMessage("La nombre del campo no puede estar vacío.")
                .NotNull().WithMessage("La nombre del campo es obligatorio.");

            RuleFor(x => x.PagoId)
                .NotNull().WithMessage("El Id del Pago es obligatorio.");
           
            RuleFor(x => x.Tipo)
               .NotEmpty().WithMessage("El Tipo no puede estar vacío.")
               .NotNull().WithMessage("El Tipo del campo es obligatorio.");
           
            RuleFor(x => x.Longitud)
                .GreaterThan(0).WithMessage("La Longitud debe ser mayor a cero.");

        }
    }
}
