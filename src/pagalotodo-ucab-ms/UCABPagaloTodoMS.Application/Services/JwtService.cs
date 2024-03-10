using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MassTransit.JobService.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Services
{
    /// <summary>
    /// Clase que maneja la creación y validación de tokens JWT.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Genera un token JWT para un usuario y una lista de roles dados.
        /// </summary>
        /// <param name="usuario">El usuario para el que se generará el token.</param>
        /// <param name="roles">La lista de roles que se asignarán al usuario en el token.</param>
        /// <returns>El token JWT generado.</returns>
        public string GenerateJwtToken(Usuario usuario,List<string> roles)
        {
            try
            {
                var jwtSecret = _config["JwtOptions:Secret"];
                var jwtIssuer = _config["JwtOptions:Issuer"];
                var jwtAudience = _config["JwtOptions:Audience"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, string.Join(",", roles)),

                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = jwtIssuer,
                    Audience = jwtAudience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch
            {
                throw new Exception($"Ocurrió un error en JWservice de login");

            }

        }

       
        /// <summary>
        /// Valida un token JWT y devuelve el principio de seguridad asociado al token.
        /// </summary>
        /// <param name="token">El token JWT a validar.</param>
        /// <returns>El principio de seguridad asociado al token.</returns>
        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var jwtSecret = _config["JwtOptions:Secret"];
            var jwtIssuer = _config["JwtOptions:Issuer"];
            var jwtAudience = _config["JwtOptions:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true, // Agrega esta línea para validar la vigencia del token
                ClockSkew = TimeSpan.Zero, // Opcional: establece el margen de tiempo permitido para la expiración del token
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            return principal;
        }
    }
}
