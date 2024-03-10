using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Core.Database
{
    /// <summary>
    /// Interfaz que define el contexto de base de datos para UCABPagaloTodoMS.
    /// </summary>
    public interface IUCABPagaloTodoDbContext
    {
        /// <summary>
        /// Obtiene o establece el DbSet de Usuarios.
        /// </summary>
        DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Obtiene o establece el DbSet de Consumidores.
        /// </summary>
        DbSet<Consumidor> Consumidores { get; set; }

        /// <summary>
        /// Obtiene o establece el DbSet de Prestadores de Servicio.
        /// </summary>
        DbSet<PrestadorServicio> PrestadorServicios { get; set; }

        /// <summary>
        /// Obtiene o establece el DbSet de Administradores.
        /// </summary>
        DbSet<Administrador> Administradores { get; set; }

        /// <summary>
        /// Obtiene o establece el DbSet de Servicios Prestados.
        /// </summary>
        DbSet<ServicioPrestado> ServiciosPrestados { get; set; }

        /// <summary>
        /// Obtiene o establece el DbSet de Pagos.
        /// </summary>
        DbSet<Pago> Pagos { get; set; }

        /// <summary>
        /// Obtiene o establece el DbSet de Configuraciones de Pago.
        /// </summary>
        DbSet<ConfiguracionPago> ConfiguracionPagos { get; set; }

        /// <summary>
        /// Obtiene o establece el DbSet de Archivos de Conciliación.
        /// </summary>
        DbSet<ArchivoConciliacion> ArchivoConciliacions { get; set; } 
        
        /// <summary>
        /// Obtiene o establece el DbSet de Archivos de deudas.
        /// </summary>
        DbSet<ArchivoDeuda> ArchivoDeudas { get; set; }
       
        /// <summary>
        /// Obtiene o establece el DbSet de Campos.
        /// </summary>
        DbSet<Campo> Campos { get; set; } 
        
        /// <summary>
        /// Obtiene o establece el DbSet de Campos.
        /// </summary>
        DbSet<CampoPago> CamposPago { get; set; }

        /// <summary>
        /// Obtiene el DbContext subyacente.
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// Inicia una nueva transacción de base de datos.
        /// </summary>
        /// <returns>Proxy de transacción de base de datos.</returns>
        IDbContextTransactionProxy BeginTransaction();

        /// <summary>
        /// Cambia el estado de la entidad en el contexto de base de datos.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de entidad.</typeparam>
        /// <param name="entity">Entidad a la que se le cambiará el estado.</param>
        /// <param name="state">Nuevo estado de la entidad.</param>
        void ChangeEntityState<TEntity>(TEntity entity, EntityState state);

        /// <summary>
        /// Guarda los cambios en el contexto de base de datos.
        /// </summary>
        /// <param name="user">Usuario que realiza la operación.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>true si los cambios se guardaron correctamente, de lo contrario, false.</returns>
        Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Guarda los cambios asincrónicamente en el contexto de base de datos.
        /// </summary>
        /// <param name="app">Nombre de la aplicación.</param>
        /// <returns>Número de entidades afectadas.</returns>
        Task<int> SaveChangesAsync(string app);
    }
}
