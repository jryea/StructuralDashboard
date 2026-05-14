namespace StructuralDashboard.Api.Repositories.ModelData;

public class FloorData
{
    private readonly AppDbContext _context;

    public FloorData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Floor>> GetFloorsAsync(string modelId)
    {
        var entities = await _context.Floors
            .Where(f => f.ModelId == modelId).ToListAsync();

        var floors = entities.Select(f => new Floor
        {
            Id = f.Id,
            LevelId = f.LevelId,
            FloorPropertiesId = f.FloorPropertiesId,
            Points = f.Points,
            DiaphragmId = f.DiaphragmId,
            SurfaceLoadId = f.SurfaceLoadId,
            SpanDirection = f.SpanDirection,
            ShellModifiers = f.ShellModifiers is null ? new ShellModifiers() : new ShellModifiers
            {
                F11 = f.ShellModifiers.F11,
                F22 = f.ShellModifiers.F22,
                F12 = f.ShellModifiers.F12,
                M11 = f.ShellModifiers.M11,
                M22 = f.ShellModifiers.M22,
                M12 = f.ShellModifiers.M12,
                V13 = f.ShellModifiers.V13,
                V23 = f.ShellModifiers.V23,
                Mass = f.ShellModifiers.Mass,
                Weight = f.ShellModifiers.Weight
            }
        }).ToList();

        return floors;
    }

    public void SaveFloors(string modelId, List<Floor> floors, HashSet<string> validFloorPropertyIds)
    {
        var entities = floors.Select(f => new FloorEntity
        {
            Id = f.Id,
            ModelId = modelId,
            LevelId = f.LevelId,
            FloorPropertiesId = validFloorPropertyIds.Contains(f.FloorPropertiesId ?? "") ? f.FloorPropertiesId : null,
            Points = f.Points,
            // Diaphragm and SurfaceLoad references skipped — those entities are stubs for now
            DiaphragmId = null,
            SurfaceLoadId = null,
            SpanDirection = f.SpanDirection,
            ShellModifiers = f.ShellModifiers is null ? null : new ShellModifiersEntity
            {
                F11 = f.ShellModifiers.F11,
                F22 = f.ShellModifiers.F22,
                F12 = f.ShellModifiers.F12,
                M11 = f.ShellModifiers.M11,
                M22 = f.ShellModifiers.M22,
                M12 = f.ShellModifiers.M12,
                V13 = f.ShellModifiers.V13,
                V23 = f.ShellModifiers.V23,
                Mass = f.ShellModifiers.Mass,
                Weight = f.ShellModifiers.Weight
            }
        }).ToList();

        _context.Floors.AddRange(entities);
    }
}