using AutoMapper;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Application.MappingProfiles
{
    /// <summary>
    /// Perfil de mapeo para la clase Usuario.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class UsuarioProfile : Profile
    {
        /// <summary>
        /// Constructor del perfil de mapeo de Usuario.
        /// </summary>
        public UsuarioProfile()
        {
            CreateMap<UsuarioResponse, Usuario>();
        }
    }
}