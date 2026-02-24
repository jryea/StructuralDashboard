using Microsoft.EntityFrameworkCore;
using StructuralDashboard.Shared.Entities;

namespace StructuralDashboard.Api.Data;

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Beam>().OwnsOne(b => b.StartPoint);
        modelBuilder.Entity<Beam>().OwnsOne(b => b.EndPoint);

        modelBuilder.Entity<Model>()
            .HasOne(m => m.Project)
            .WithMany()
            .HasForeignKey(m => m.ProjectNumber);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; } 
    public DbSet<Model> Models { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Beam> Beams { get; set; }
    public DbSet<FrameProperties> FrameProperties { get; set; }
}
