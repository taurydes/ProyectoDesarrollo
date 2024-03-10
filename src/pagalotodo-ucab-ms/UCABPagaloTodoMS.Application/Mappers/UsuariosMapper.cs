using Automatonymous;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Mappers
{
    /// <summary>
    /// Clase estática que proporciona métodos para mapear entidades relacionadas con usuarios.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public static class UsuarioMapper
    {
        /// <summary>
        /// Mapea una entidad Usuario a un objeto UsuarioResponse.
        /// </summary>
        /// <param name="entity">La entidad Usuario.</param>
        /// <returns>Un objeto UsuarioResponse mapeado.</returns>
        public static UsuarioResponse MapEntityAResponse(Usuario entity)
        {
            var response = new UsuarioResponse()
            {
                Id = entity.Id,
                NombreUsuario = entity.NombreUsuario,
                Clave = entity.Clave,
                Correo = entity.Correo
            };
            return response;
        }

        /// <summary>
        /// Mapea una entidad Consumidor a un objeto ConsumidorResponse.
        /// </summary>
        /// <param name="entity">La entidad Consumidor.</param>
        /// <returns>Un objeto ConsumidorResponse mapeado.</returns>
        public static ConsumidorResponse MapEntityConsumidorAResponse(Consumidor entity)
        {
            var response = new ConsumidorResponse()
            {
                //datos de Heredados de Usuario
                Id = entity.Id,
                NombreUsuario = entity.NombreUsuario,
                Clave = entity.Clave,
                Correo = entity.Correo,

                // datos Propios del consumidor 
                Cedula = entity.Cedula,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                Telefono = entity.Telefono,
                Direccion = entity.Direccion,
                PagosRealizados = entity.PagosRealizados,
                EstatusCuenta = entity.EstatusCuenta,

            };
            return response;
        }
        /// <summary>
        /// Mapea una entidad PrestadorServicio a un objeto PrestadorServicioResponse.
        /// </summary>
        /// <param name="entity">La entidad PrestadorServicio.</param>
        /// <returns>Un objeto PrestadorServicioResponse mapeado.</returns>
        public static PrestadorServicioResponse MapEntityPrestadorServicioAResponse(PrestadorServicio entity)
        {
            var response = new PrestadorServicioResponse()
            {
                //datos de Heredados de Usuario
                Id = entity.Id,
                NombreUsuario = entity.NombreUsuario,
                Clave = entity.Clave,
                Correo = entity.Correo,

                // datos Propios del consumidor 
                NombreEmpresa = entity.NombreEmpresa,
                Rif = entity.Rif,
                ServiciosPrestados = entity.ServiciosPrestados,
                EstatusCuenta = entity.EstatusCuenta,


            };
            return response;
        }

        /// <summary>
        /// Mapea una entidad Administrador a un objeto AdministradorResponse.
        /// </summary>
        /// <param name="entity">La entidad Administrador.</param>
        /// <returns>Un objeto AdministradorResponse mapeado.</returns>
        public static AdministradorResponse MapEntityAdministradorAResponse(Administrador entity)
        {
            var response = new AdministradorResponse()
            {
                //datos de Heredados de Usuario
                Id = entity.Id,
                NombreUsuario = entity.NombreUsuario,
                Clave = entity.Clave,
                Correo = entity.Correo,

                // datos Propios del consumidor 
                NombreAdministrador = entity.NombreAdministrador,
             
            };
            return response;
        }

        /// <summary>
        /// Mapea una solicitud UsuarioRequest a una entidad Usuario.
        /// </summary>
        /// <param name="request">La solicitud UsuarioRequest.</param>
        /// <returns>Una entidad Usuario mapeada.</returns>
        public static Usuario MapRequestEntity(UsuarioRequest request)
        {
            var entity = new Usuario()
            {
                NombreUsuario = request.NombreUsuario,
                Clave = request.Clave,
                Correo = request.Correo ?? string.Empty
            };
            return entity;
        }

        /// <summary>
        /// Mapea una solicitud ConsumidorRequest a una entidad Consumidor.
        /// </summary>
        /// <param name="request">La solicitud ConsumidorRequest.</param>
        /// <returns>Una entidad Consumidor mapeada.</returns>
        public static Consumidor MapRequestConsumidorEntity(ConsumidorRequest request)
        {
            var entity = new Consumidor()
            {
                Cedula = request.Cedula,
                NombreUsuario = request.NombreUsuario,
                Clave = request.Clave,
                Correo = request.Correo ?? string.Empty,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Direccion = request.Direccion,

            };
            return entity;
        }

        /// <summary>
        /// Mapea una solicitud PrestadorServicioRequest a una entidad PrestadorServicio.
        /// </summary>
        /// <param name="request">La solicitud PrestadorServicioRequest.</param>
        /// <returns>Una entidad PrestadorServicio mapeada.</returns>
        public static PrestadorServicio MapRequestPrestadorServicioEntity(PrestadorServicioRequest request)
        {
            var entity = new PrestadorServicio()
            {
                NombreUsuario = request.NombreUsuario,
                Clave = request.Clave,
                Correo = request.Correo ?? string.Empty,
                NombreEmpresa = request.NombreEmpresa,
                Rif = request.Rif
              
            };
            return entity;
        }

        /// <summary>
        /// Mapea una solicitud AdministradorRequest a una entidad Administrador.
        /// </summary>
        /// <param name="request">La solicitud AdministradorRequest.</param>
        /// <returns>Una entidad Administrador mapeada.</returns>
        public static Administrador MapRequestAdministradorEntity(AdministradorRequest request)
        {
            var entity = new Administrador()
            {
                NombreUsuario = request.NombreUsuario,
                Clave = request.Clave,
                Correo = request.Correo ?? string.Empty,
                NombreAdministrador = request.NombreAdministrador,
                
            };
            return entity;
        }

                
    }
}
