using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    namespace UCABPagaloTodoMS.Application.Validators
    {
        public class ModeloArchivoConciliacionValidator : AbstractValidator<CrearModeloArchivoConciliacionRequest>
        {
            public ModeloArchivoConciliacionValidator()
            {
                RuleFor(x => x.NombreModelo)
                    .NotEmpty().WithMessage("El nombre del modelo de archivo es requerido.");

                RuleFor(x => x.NombreCampo)
                    .NotNull().WithMessage("El campo 'NombreCampo' es requerido.");

                RuleFor(x => x.Tipo)
                    .NotNull().WithMessage("El campo 'Tipo' es requerido.");

                RuleFor(x => x.Longitud)
                    .NotNull().WithMessage("El campo 'Longitud' es requerido.");

                RuleFor(x => x.Referencia)
                    .NotNull().WithMessage("El campo 'Referencia' es requerido.");

                RuleFor(x => x.Monto)
                    .NotNull().WithMessage("El campo 'Monto' es requerido.");
            }
        }
    }
}