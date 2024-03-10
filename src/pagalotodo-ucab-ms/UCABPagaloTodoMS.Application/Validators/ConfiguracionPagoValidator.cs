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
    /// Validador para la creación de una configuracion de servicios.
    /// </summary>
    public class ConfiguracionPagoValidator : AbstractValidator<CrearConfiguracionPagoRequest>
    {
        public ConfiguracionPagoValidator()
        {
               RuleFor(x => x.ServicioPrestadoId)
                .NotNull().WithMessage("El Id del servicio prestado es obligatorio.");
        }
    }
}
