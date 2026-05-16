using Microsoft.EntityFrameworkCore;
using StructuralDashboard.Shared.Entities;

namespace StructuralDashboard.Api.Data;

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BeamEntity>().OwnsOne(b => b.StartPoint);
        modelBuilder.Entity<BeamEntity>().OwnsOne(b => b.EndPoint);
        modelBuilder.Entity<BeamEntity>().OwnsOne(b => b.FrameModifiers);
        modelBuilder.Entity<BraceEntity>().OwnsOne(b => b.StartPoint);
        modelBuilder.Entity<BraceEntity>().OwnsOne(b => b.EndPoint);
        modelBuilder.Entity<BraceEntity>().OwnsOne(b => b.FrameModifiers);
        modelBuilder.Entity<ColumnEntity>().OwnsOne(c => c.StartPoint);
        modelBuilder.Entity<ColumnEntity>().OwnsOne(c => c.EndPoint);
        modelBuilder.Entity<ColumnEntity>().OwnsOne(c => c.FrameModifiers);
        modelBuilder.Entity<WallEntity>().OwnsOne(w => w.StartPoint);
        modelBuilder.Entity<WallEntity>().OwnsOne(w => w.EndPoint);
        modelBuilder.Entity<GridEntity>().OwnsOne(g => g.StartPoint);
        modelBuilder.Entity<GridEntity>().OwnsOne(g => g.EndPoint);
        modelBuilder.Entity<IsolatedFootingEntity>().OwnsOne(f => f.Point);

        modelBuilder.Entity<FloorEntity>().OwnsOne(f => f.ShellModifiers);
        modelBuilder.Entity<FloorPropertiesEntity>().OwnsOne(fp => fp.DeckProperties);
        modelBuilder.Entity<FloorPropertiesEntity>().OwnsOne(fp => fp.ShearStudProperties);
        modelBuilder.Entity<FloorPropertiesEntity>().OwnsOne(fp => fp.ShellModifiers);

        modelBuilder.Entity<FramePropertiesEntity>().OwnsOne(fp => fp.ConcreteProps);
        modelBuilder.Entity<FramePropertiesEntity>().OwnsOne(fp => fp.SteelProps);
        modelBuilder.Entity<FramePropertiesEntity>().OwnsOne(fp => fp.WoodProps);
        modelBuilder.Entity<FramePropertiesEntity>().OwnsOne(fp => fp.FrameModifiers);

        modelBuilder.Entity<MaterialEntity>().OwnsOne(m => m.ConcreteProps);
        modelBuilder.Entity<MaterialEntity>().OwnsOne(m => m.SteelProps);
        modelBuilder.Entity<WallPropertiesEntity>().OwnsOne(wp => wp.ETABSModifiers);

        modelBuilder.Entity<StructuralModelEntity>()
            .HasOne(m => m.Project)
            .WithMany()
            .HasForeignKey(m => m.ProjectNumber);

        modelBuilder.Entity<FloorEntity>().OwnsMany(f => f.Points).ToJson();
        modelBuilder.Entity<OpeningEntity>().OwnsMany(f=> f.Points).ToJson();

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<ProjectEntity> Projects { get; set; } 
    public DbSet<StructuralModelEntity> Models { get; set; }
    public DbSet<LevelEntity> Levels { get; set; }
    public DbSet<GridEntity> Grids { get; set; }
    public DbSet<BeamEntity> Beams { get; set; }
    public DbSet<BraceEntity> Braces { get; set; }
    public DbSet<ColumnEntity> Columns { get; set; }
    public DbSet<IsolatedFootingEntity> Footings { get; set; }
    public DbSet<FloorEntity> Floors { get; set; }
    public DbSet<WallEntity> Walls { get; set; }
    public DbSet<OpeningEntity> Openings { get; set; }
    public DbSet<FloorPropertiesEntity> FloorProperties { get; set; }
    public DbSet<FramePropertiesEntity> FrameProperties { get; set; }
    public DbSet<WallPropertiesEntity> WallProperties{ get; set; }
    public DbSet<MaterialEntity> Materials { get; set; }
}
