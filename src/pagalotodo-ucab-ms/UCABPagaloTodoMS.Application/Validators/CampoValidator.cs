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
    /// Validador para la creación de un campo.
    /// </summary>
    public class CampoValidator : AbstractValidator<CrearCampoRequest>
    {
        /// <summary>
        /// Crea una nueva instancia del validador de campo.
        /// </summary>
        public CampoValidator()
        {

            RuleFor(x => x.NombreCampo)
                .NotEmpty().WithMessage("La nombre del campo no puede estar vacío.")
                .NotNull().WithMessage("La nombre del campo es obligatorio.");

            RuleFor(x => x.ConfiguracionPagoId)
                .NotNull().WithMessage("El Id  configuración del Pago es obligatorio.");
           
            RuleFor(x => x.Tipo)
               .NotEmpty().WithMessage("El Tipo no puede estar vacío.")
               .NotNull().WithMessage("El Tipo del campo es obligatorio.");
           
            RuleFor(x => x.Longitud)
                .GreaterThan(0).WithMessage("La Longitud debe ser mayor a cero.");

        }
    }
}
