using System.Reflection;
using Filio.Api.Abstractions;
using Filio.Api.Domains;
using Microsoft.EntityFrameworkCore;

namespace Filio.Api.Data;

/// <summary>
/// Filio DataContext 
/// </summary>
public class DataContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextOptions"></param>
    /// <returns></returns>
    public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions) { }


    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("hstore");
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Model.GetEntityTypes()
                            .ToList()
                            .ForEach(x => x.SetTableName("Filio_" + x.GetTableName()));

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    ///<inheritdoc />
    public override int SaveChanges()
    {
        ApplyBeforeSave();
        return base.SaveChanges();
    }

    ///<inheritdoc />
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    ///<inheritdoc />
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyBeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc />
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// File entities
    /// </summary>
    /// <value></value>
    public DbSet<FileDomain> FileDomains { get; set; } = default!;


    /// <summary>
    /// The function calls before saving changes
    /// </summary>
    private void ApplyBeforeSave()
    {
        ChangeTracker.DetectChanges();

        foreach (var entity in ChangeTracker.Entries())
        {
            if (entity.Entity is BaseEntity baseEntity)
            {
                if (entity.State == EntityState.Added)
                {
                    baseEntity.OnBaseCreate();
                }
                else if (entity.State == EntityState.Modified)
                {
                    baseEntity.OnBaseUpdate();
                }
            }
        }
    }
}