using Microsoft.EntityFrameworkCore;
using VebTech.Domain.Models;
using VebTech.Domain.Models.Configurations;
using VebTech.Infrastructure.Context.SeedData;

namespace VebTech.Infrastructure.Context;
public class AppDbContext : DbContext
{
    private readonly AdminConfig _adminConfig;
    public AppDbContext(DbContextOptions<AppDbContext> options,
        AdminConfig adminConfig) : base(options)
    {
        _adminConfig = adminConfig;
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SeedUsers();
        modelBuilder.SeedRoles();
        modelBuilder.SeedAdmin(_adminConfig);

        modelBuilder.Entity<Role>()
        .HasOne(r => r.User)
        .WithMany(u => u.Roles)
        .OnDelete(DeleteBehavior.Cascade);
    }
}