using System.Linq.Expressions;
using System.Reflection;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace UCABPagaloTodoMS.Infrastructure.Database;


public class UCABPagaloTodoDbContext : DbContext, IUCABPagaloTodoDbContext
{
    public UCABPagaloTodoDbContext(DbContextOptions<UCABPagaloTodoDbContext> options)
        : base(options)
    {
    }

    
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<ArchivoConciliacion> ArchivoConciliacions { get; set; }
    public virtual DbSet<Administrador> Administradores { get; set; }
    public virtual DbSet<Consumidor> Consumidores { get; set; }
    public virtual DbSet<Pago> Pagos { get; set; }
    public virtual DbSet<PrestadorServicio> PrestadorServicios { get; set; }
    public virtual DbSet<ServicioPrestado> ServiciosPrestados { get; set; }
    public virtual DbSet<ConfiguracionPago> ConfiguracionPagos { get; set; }
    public virtual DbSet<Campo> Campos { get; set; }
    public virtual DbSet<CampoPago> CamposPago { get; set; }
    public virtual DbSet<ArchivoDeuda> ArchivoDeudas { get; set; }
   



    public DbContext DbContext
    {
        get
        {
            return this;
        }
    }

    public object Usuario => throw new NotImplementedException();

  
    public IDbContextTransactionProxy BeginTransaction()
    {
        return new DbContextTransactionProxy(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // agregado para poder usar la relación uno a muchos entre servicios prestados y prestador: 
        // esto se usa cuando no se usa en entityFramework--- 
        // Relación uno-a-muchos entre PrestadorServicio y ServicioPrestado
        //modelBuilder.Entity<ServicioPrestado>()
        //    .HasOne<PrestadorServicio>()
        //    .WithMany(p => p.ServiciosPrestados)
        //    .HasForeignKey(s => s.PrestadorServicioId);
        //

    }

    virtual public void SetPropertyIsModifiedToFalse<TEntity, TProperty>(TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression) where TEntity : class
    {
        Entry(entity).Property(propertyExpression).IsModified = false;
    }

    virtual public void ChangeEntityState<TEntity>(TEntity entity, EntityState state)
    {
        if (entity != null)
        {
            Entry(entity).State = state;
        }
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                Entry((BaseEntity)entityEntry.Entity).Property(x => x.CreatedAt).IsModified = false;
                Entry((BaseEntity)entityEntry.Entity).Property(x => x.CreatedBy).IsModified = false;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(string user, CancellationToken cancellationToken = default)
    {
        var state = new List<EntityState> { EntityState.Added, EntityState.Modified };

        var entries = ChangeTracker.Entries().Where(e =>
            e.Entity is BaseEntity && state.Any(s => e.State == s)
        );

        var dt = DateTime.UtcNow;

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = dt;
                entity.CreatedBy = user;
                Entry(entity).Property(x => x.UpdatedAt).IsModified = false;
                Entry(entity).Property(x => x.UpdatedBy).IsModified = false;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entity.UpdatedAt = dt;
                entity.UpdatedBy = user;
                Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    // PRUEBA 
    public async Task<int> SaveChangesAsync(string app)
    {
        return await base.SaveChangesAsync();
    }
    //
    public async Task<bool> SaveEfContextChanges(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) >= 0;
    }

    public async Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(user, cancellationToken) >= 0;
    }
}
