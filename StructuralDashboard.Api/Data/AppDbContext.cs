using Microsoft.EntityFrameworkCore;
using StructuralDashboard.Shared.Entities;

namespace StructuralDashboard.Api.Data;

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Beam>().OwnsOne(b => b.StartPoint);
        modelBuilder.Entity<Beam>().OwnsOne(b => b.EndPoint);
        modelBuilder.Entity<Beam>().OwnsOne(b => b.FrameModifiers);
        modelBuilder.Entity<Brace>().OwnsOne(b => b.StartPoint);
        modelBuilder.Entity<Brace>().OwnsOne(b => b.EndPoint);
        modelBuilder.Entity<Brace>().OwnsOne(b => b.FrameModifiers);
        modelBuilder.Entity<Column>().OwnsOne(c => c.StartPoint);
        modelBuilder.Entity<Column>().OwnsOne(c => c.EndPoint);
        modelBuilder.Entity<Column>().OwnsOne(c => c.FrameModifiers);
        modelBuilder.Entity<Wall>().OwnsOne(w => w.StartPoint);
        modelBuilder.Entity<Wall>().OwnsOne(w => w.EndPoint);
        modelBuilder.Entity<Grid>().OwnsOne(g => g.StartPoint);
        modelBuilder.Entity<Grid>().OwnsOne(g => g.EndPoint);
        modelBuilder.Entity<IsolatedFooting>().OwnsOne(f => f.Point);

        modelBuilder.Entity<Floor>().OwnsOne(f => f.ShellModifiers);
        modelBuilder.Entity<FloorProperties>().OwnsOne(fp => fp.ShearStudProperties);
        modelBuilder.Entity<FloorProperties>().OwnsOne(fp => fp.ShellModifiers);

        modelBuilder.Entity<FrameProperties>().OwnsOne(fp => fp.ConcreteProps);
        modelBuilder.Entity<FrameProperties>().OwnsOne(fp => fp.SteelProps);
        modelBuilder.Entity<FrameProperties>().OwnsOne(fp => fp.WoodProps);
        modelBuilder.Entity<FrameProperties>().OwnsOne(fp => fp.FrameModifiers);

        modelBuilder.Entity<Model>()
            .HasOne(m => m.Project)
            .WithMany()
            .HasForeignKey(m => m.ProjectNumber);

        modelBuilder.Entity<Floor>().OwnsMany(f => f.Points).ToJson();
        modelBuilder.Entity<Opening>().OwnsMany(f=> f.Points).ToJson();

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
    public DbSet<Grid> Grids { get; set; }
    public DbSet<Beam> Beams { get; set; }
    public DbSet<Brace> Braces { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<IsolatedFooting> Footings { get; set; }
    public DbSet<Floor> Floors { get; set; }
    public DbSet<Wall> Walls { get; set; }
    public DbSet<Opening> Openings { get; set; }
    public DbSet<FloorProperties> FloorProperties { get; set; }
    public DbSet<FrameProperties> FrameProperties { get; set; }
    public DbSet<WallProperties> WallProperties{ get; set; }
    public DbSet<Material> Materials { get; set; }
}
