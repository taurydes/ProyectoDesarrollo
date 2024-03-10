using MediatR;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands.FeatureArchivoConciliacion;
using UCABPagaloTodoMS.Application.Services;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureArchivoConciliacion
{
    /// <summary>
    /// Clase que maneja el comando para guardar la respuesta en un archivo de texto.
    /// </summary>
    public class GuardarRespuestaEnArchivoDeTextoCommandAprobadoHandler : IRequestHandler<GuardarRespuestaEnArchivoDeTextoAprobadoCommand, string>
    {
        private readonly ILogger<GuardarRespuestaEnArchivoDeTextoCommandAprobadoHandler> _logger;
        private readonly FirebaseStorageUploader _firebaseStorageUploaderService;
        private readonly IMailService _mailService;

        public GuardarRespuestaEnArchivoDeTextoCommandAprobadoHandler(ILogger<GuardarRespuestaEnArchivoDeTextoCommandAprobadoHandler> logger, IMailService mailService)
        {
            _logger = logger;
            _firebaseStorageUploaderService = new FirebaseStorageUploader();
            _mailService = mailService;
        }

        /// <summary>
        /// Método Handle que procesa la solicitud de creación del archivo y crea un archivo CSV temporal en memoria para luego ser subido a Firebase Storage.
        /// También envía el URL de descarga del archivo al correo electrónico del prestador si se configura el servicio de correo electrónico.
        /// </summary>
        /// <param name="request">La solicitud de creación del archivo</param>
        /// <param name="cancellationToken"></param>
        /// <returns>El URL de descarga del archivo en Firebase Storage</returns>
        public async Task<string> Handle(GuardarRespuestaEnArchivoDeTextoAprobadoCommand request, CancellationToken cancellationToken)
        {
            string downloadUrl = "";
            var nombreEmpresa = request.Response.NombreEmpresa;
            var nombreServicio = request.Response.Nombre;
            var pagosRealizados = request.Response.PagosPorServicioRealizados;

            // Crear el archivo CSV en memoria
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    // Escribir el encabezado del archivo
                    var camposEncabezado = new List<string>() { "Nombre Empresa", "Nombre Servicio", "Pago", "Referencia", "Total" };
                    camposEncabezado.AddRange(pagosRealizados.SelectMany(p => p.CamposPago.Select(c => c.NombreCampo)).Distinct());
                    await writer.WriteLineAsync($"\"{string.Join("\", \"", camposEncabezado)}\"");

                    // Escribir cada pago
                    int contadorPago = 0;
                    foreach (var pago in pagosRealizados)
                    {
                        contadorPago++;
                        // Escribir el pago
                        var camposPago = new Dictionary<string, string>();
                        foreach (var campo in pago.CamposPago)
                        {
                            camposPago[campo.NombreCampo] = campo.Valor;
                        }
                        var sbPago = new StringBuilder();
                        sbPago.Append($"\"{nombreEmpresa}\",");
                        sbPago.Append($"\"{nombreServicio}\",");
                        sbPago.Append($"{contadorPago},");
                        sbPago.Append($"\"{pago.Referencia}\",");
                        sbPago.Append($"\"\",");
                        foreach (var campo in camposEncabezado.Skip(5))
                        {
                            sbPago.Append($"\"{(camposPago.ContainsKey(campo) ? camposPago[campo] : "")}\",");
                        }
                        sbPago.Append($"\"{(camposPago.ContainsKey("Total") ? camposPago["Total"] : "")}\","); // Agregar el campo "Total" al final de la línea de pago
                        await writer.WriteLineAsync(sbPago.ToString());
                    }

                    // Escribir el total de los montos al final del archivo
                    var totalMonto = pagosRealizados.Sum(p => decimal.Parse(p.CamposPago.First(c => c.NombreCampo == "Monto").Valor));
                    var sbTotal = new StringBuilder();
                    sbTotal.Append("\"Total\",");
                    sbTotal.Append("\"\",");
                    sbTotal.Append("\"\",");
                    sbTotal.Append($"\"\",");
                    sbTotal.Append($"\"{totalMonto.ToString("C")}\",");
                    sbTotal.Append("\"\","); // Dejar un campo vacío para "Referencia"
                    sbTotal.Append("\"\""); 
                    await writer.WriteLineAsync(sbTotal.ToString());
                }

                //Subir el archivo a Firebase Storage y guardar la URL en la base de datos
                var fileName = $"{nombreEmpresa}-{nombreServicio}";
                downloadUrl = await _firebaseStorageUploaderService.UploadFileAsync("ArchivosConciliacionAprobados/" + fileName, Encoding.UTF8.GetString(stream.ToArray()));

                // Enviar el correo electrónico con el URL de descarga si se configura el servicio de correo electrónico
                var asuntoMensaje = $"Cierre Contable Aprobado de su servicio {nombreServicio}. PagalotodoUCAB";
                var cuerpoMensaje = $"Hola {nombreEmpresa} se ha generado el cierre contable. Haga click en este link para descargar su archivo de conciliacion :  {downloadUrl}   Att:PagaloTodoUCAB.";

                // Enviar el correo electrónico con el URL de descarga si se configura el servicio de correo electrónico
                await _mailService.EnviarCorreoElectronicoAsync(request.Response.Correo ?? "dtoro1996@gmail.com", asuntoMensaje, cuerpoMensaje);
            }

            return downloadUrl;
        }
    }
}