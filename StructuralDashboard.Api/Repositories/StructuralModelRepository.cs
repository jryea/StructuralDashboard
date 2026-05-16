using StructuralDashboard.Api.Repositories.ModelData;

namespace StructuralDashboard.Api.Repositories;

public class StructuralModelRepository : IStructuralModelRepository
{
    private readonly AppDbContext _context;

    public StructuralModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateModelAsync(StructuralModel model, string projectNumber)
    {
        var modelEntity = new StructuralModelEntity()
        {
            Id = model.Id,
            ModelName = model.Metadata.ProjectInfo.ModelName,
            ProjectNumber = projectNumber,
            SavedBy = model.Metadata.ProjectInfo.SavedBy,
            SavedAtUtc = DateTime.UtcNow,

            // Placholder - StructuralModel doesn't contain source application data yet
            SourceApplication = Shared.Enums.SourceApplication.Revit
        };

        _context.Models.Add(modelEntity);

        BuildModelEntities(model);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteModelAsync(string id)
    {
        var modelEntity = await _context.Models.FindAsync(id);
        if (modelEntity is null) return;

        _context.Models.Remove(modelEntity);

        RemoveModelEntities(id);

        await _context.SaveChangesAsync();
    }

    public async Task<List<StructuralModel>> GetAllModelsAsync(string projectNumber)
    {
        var modelEntities = await _context.Models
            .Where(m => m.ProjectNumber == projectNumber)
            .ToListAsync();

        var tasks = modelEntities.Select(m => BuildStructuralModelAsync(m));
        var models = await Task.WhenAll(tasks);

        return models.ToList();
    }

    public async Task<StructuralModel?> GetModelAsync(string id)
    {
        var entity = await _context.Models.FindAsync(id);

        if (entity is null) return null;

        var structuralModel = await BuildStructuralModelAsync(entity);

        return structuralModel;
    }

    public async Task UpdateModelAsync(StructuralModel model)
    {
        var modelEntity = await _context.Models.FindAsync(model.Id);

        if (modelEntity is null) return;

        modelEntity.ModelName = model.Metadata.ProjectInfo.ModelName;
        modelEntity.SavedBy = model.Metadata.ProjectInfo.SavedBy;
        modelEntity.SavedAtUtc = DateTime.UtcNow;

        RemoveModelEntities(model.Id);

        BuildModelEntities(model);

        await _context.SaveChangesAsync();
    }

    private async Task<StructuralModel> BuildStructuralModelAsync(StructuralModelEntity entity)
    {
        // instantiate data classes
        var levelData = new LevelData(_context);
        var gridData = new GridData(_context);
        var materialData = new MaterialData(_context);
        var wallPropertiesData = new WallPropertiesData(_context);
        var framePropertiesData = new FramePropertiesData(_context);
        var beamData = new BeamData(_context);
        var columnData = new ColumnData(_context);
        var braceData = new BraceData(_context);
        var wallData = new WallData(_context);

        // fetch data
        var levels = await levelData.GetLevelsAsync(entity.Id);
        var grids = await gridData.GetGridsAsync(entity.Id);
        var materials = await materialData.GetMaterialsAsync(entity.Id);
        var wallProperties = await wallPropertiesData.GetWallPropertiesAsync(entity.Id);
        var frameProperties = await framePropertiesData.GetFramePropertiesAsync(entity.Id);
        var beams = await beamData.GetBeamsAsync(entity.Id);
        var columns = await columnData.GetColumnsAsync(entity.Id);
        var braces = await braceData.GetBracesAsync(entity.Id);
        var walls = await wallData.GetWallsAsync(entity.Id);

        // assemble
        var structuralModel = new StructuralModel
        {
            Id = entity.Id,
            ModelLayout = new ModelLayoutContainer
            {
                Levels = levels,
                Grids = grids
            },
            Properties = new PropertiesContainer
            {
                Materials = materials,
                WallProperties = wallProperties,
                FrameProperties = frameProperties
            },
            Elements = new ElementContainer
            {
                Beams = beams,
                Columns = columns,
                Braces = braces,
                Walls = walls
            }
        };

        return structuralModel;
    }

    private void BuildModelEntities(StructuralModel model)
    {
        // decompose model
        var levels = model.ModelLayout.Levels;
        var grids = model.ModelLayout.Grids;
        var materials = model.Properties.Materials;
        var wallProperties = model.Properties.WallProperties ?? new();
        var floorProperties = model.Properties.FloorProperties ?? new();
        var frameProperties = model.Properties.FrameProperties;
        var beams = model.Elements.Beams;
        var columns = model.Elements.Columns;
        var braces = model.Elements.Braces;
        var walls = model.Elements.Walls;
        var floors = model.Elements.Floors;
        var footings = model.Elements.IsolatedFootings;
        var openings = model.Elements.Openings;

        // Sets of valid property IDs — used to null out dangling references
        var validFramePropertyIds = frameProperties.Select(fp => fp.Id).ToHashSet();
        var validWallPropertyIds = wallProperties.Select(wp => wp.Id).ToHashSet();
        var validFloorPropertyIds = floorProperties.Select(fp => fp.Id).ToHashSet();

        // instantiate data classes
        var levelData = new LevelData(_context);
        var gridData = new GridData(_context);
        var materialData = new MaterialData(_context);
        var wallPropertiesData = new WallPropertiesData(_context);
        var floorPropertiesData = new FloorPropertiesData(_context);
        var framePropertiesData = new FramePropertiesData(_context);
        var beamData = new BeamData(_context);
        var columnData = new ColumnData(_context);
        var braceData = new BraceData(_context);
        var wallData = new WallData(_context);
        var floorData = new FloorData(_context);
        var footingData = new IsolatedFootingData(_context);
        var openingData = new OpeningData(_context);

        // save data to context (parents before children)
        levelData.SaveLevels(model.Id, levels);
        gridData.SaveGrids(model.Id, grids);
        materialData.SaveMaterials(model.Id, materials);
        wallPropertiesData.SaveWallProperties(model.Id, wallProperties);
        floorPropertiesData.SaveFloorProperties(model.Id, floorProperties);
        framePropertiesData.SaveFrameProperties(model.Id, frameProperties);
        beamData.SaveBeams(model.Id, beams, validFramePropertyIds);
        columnData.SaveColumns(model.Id, columns, validFramePropertyIds);
        braceData.SaveBraces(model.Id, braces ?? new(), validFramePropertyIds);
        wallData.SaveWalls(model.Id, walls ?? new(), validWallPropertyIds);
        floorData.SaveFloors(model.Id, floors ?? new(), validFloorPropertyIds);
        footingData.SaveIsolatedFootings(model.Id, footings ?? new());
        openingData.SaveOpenings(model.Id, openings ?? new());
    }

    private void RemoveModelEntities(string modelId)
    {
        var levels = _context.Levels.Where(x => x.ModelId == modelId);
        var grids = _context.Grids.Where(x => x.ModelId == modelId);
        var materials = _context.Materials.Where(x => x.ModelId == modelId);
        var wallProperties = _context.WallProperties.Where(x => x.ModelId == modelId);
        var floorProperties = _context.FloorProperties.Where(x => x.ModelId == modelId);
        var frameProperties = _context.FrameProperties.Where(x => x.ModelId == modelId);
        var beams = _context.Beams.Where(x => x.ModelId == modelId);
        var columns = _context.Columns.Where(x => x.ModelId == modelId);
        var braces = _context.Braces.Where(x => x.ModelId == modelId);
        var walls = _context.Walls.Where(x => x.ModelId == modelId);
        var floors = _context.Floors.Where(x => x.ModelId == modelId);
        var footings = _context.Footings.Where(x => x.ModelId == modelId);
        var openings = _context.Openings.Where(x => x.ModelId == modelId);

        // remove children before parents
        _context.Beams.RemoveRange(beams);
        _context.Columns.RemoveRange(columns);
        _context.Braces.RemoveRange(braces);
        _context.Walls.RemoveRange(walls);
        _context.Floors.RemoveRange(floors);
        _context.Footings.RemoveRange(footings);
        _context.Openings.RemoveRange(openings);
        _context.FloorProperties.RemoveRange(floorProperties);
        _context.WallProperties.RemoveRange(wallProperties);
        _context.FrameProperties.RemoveRange(frameProperties);
        _context.Materials.RemoveRange(materials);
        _context.Grids.RemoveRange(grids);
        _context.Levels.RemoveRange(levels);
    }
}
