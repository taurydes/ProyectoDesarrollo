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
    /// Validador para la actualización de un campo.
    /// </summary>
    public class CampoUpdateValidator : AbstractValidator<CampoUpdateRequest>
    {
        /// <summary>
        /// Crea una nueva instancia del validador de campoUpdate.
        /// </summary>
        
        public CampoUpdateValidator()
        {

            RuleFor(x => x.NombreCampo)
                .NotEmpty().WithMessage("La nombre del campo no puede estar vacío.")
                .NotNull().WithMessage("La nombre del campo es obligatorio.");
                     
            RuleFor(x => x.Tipo)
               .NotEmpty().WithMessage("El Tipo no puede estar vacío.")
               .NotNull().WithMessage("El Tipo del campo es obligatorio.");
           
            RuleFor(x => x.Longitud)
                .GreaterThan(0).WithMessage("La Longitud debe ser mayor a cero.");

        }
    }
}
