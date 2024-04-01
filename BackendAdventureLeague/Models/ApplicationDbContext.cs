using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace BackendAdventureLeague.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    
    public DbSet<Request> Requests => Set<Request>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.Phone)
            .IsUnique();
        base.OnModelCreating(builder);
    }
}
public interface IApplicationDbContext : IInfrastructure<IServiceProvider>,
    IDbContextDependencies,
    IDbSetCache,
    IDbContextPoolable
{
    DbSet<Account> Accounts { get; }
    
    DbSet<Request> Requests { get; }
    
    DbSet<ApplicationUser> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    int SaveChanges();

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
