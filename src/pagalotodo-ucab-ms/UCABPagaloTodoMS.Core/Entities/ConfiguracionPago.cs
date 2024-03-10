using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    /// <summary>
    /// Representa la configuración de pago para un servicio prestado.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class ConfiguracionPago : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado al que pertenece la configuración de pago.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece una lista de campos que pertenecen o conforman la configuración de pago.
        /// </summary>
        public List<Campo>? Campos { get; set; }

    }
}

//investigado: 
/*Un servicio de pago suele requerir la siguiente información para procesar un pago:
1. Información de la tarjeta de crédito/débito: esto incluye el número de la tarjeta, la fecha de vencimiento, el código de seguridad (CVV) y el nombre del titular de la tarjeta.
2. Monto del pago: el monto que se va a cargar a la tarjeta de crédito/débito.
3. Dirección de facturación: la dirección a la que se debe enviar la factura del pago.
4. Información del cliente: esto puede incluir el nombre, apellido, correo electrónico, número de teléfono y dirección de envío.
5. Información de autenticación: si el servicio de pago requiere una autenticación adicional, puede ser necesario proporcionar una contraseña o un código de autenticación de dos factores.

Es importante tener en cuenta que algunos servicios de pago pueden requerir información adicional dependiendo del tipo de pago que se esté procesando o de las políticas del proveedor del servicio de pago.
*/
