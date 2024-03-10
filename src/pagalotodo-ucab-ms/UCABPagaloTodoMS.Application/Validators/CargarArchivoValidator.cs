using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Validators
{
    /// <summary>
    /// Validador para cargar un archivo.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CargarArchivoValidator : AbstractValidator<IFormFile>
    {
        /// <summary>
        /// Crea una nueva instancia del validador de carga de archivo.
        /// </summary>
        public CargarArchivoValidator()
        {
            RuleFor(archivo => archivo)
                .NotNull().WithMessage("El archivo no puede ser nulo.")
                .NotEmpty().WithMessage("El archivo no puede estar vacío.")
                .Must(EsTipoValido).WithMessage("El archivo debe ser un PDF o un archivo de texto.");

            RuleFor(archivo => archivo.Length)
                .GreaterThan(0).WithMessage("El archivo no puede estar vacío.");
        }

        /// <summary>
        /// Verifica si el tipo de archivo es válido.
        /// </summary>
        /// <param name="archivo">Archivo a verificar.</param>
        /// <returns>True si el tipo de archivo es válido, de lo contrario, False.</returns>
        private bool EsTipoValido(IFormFile archivo)
        {
            // Obtener el tipo MIME del archivo
            var tipoMIME = archivo.ContentType;

            // Verificar si el tipo MIME es un PDF o un archivo de texto
            return tipoMIME == "application/pdf" || tipoMIME == "text/plain";
        }
    }
}