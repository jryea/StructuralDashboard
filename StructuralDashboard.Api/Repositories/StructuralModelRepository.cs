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
        var framePropertiesData = new FramePropertiesData(_context);
        var beamData = new BeamData(_context);
        var columnData = new ColumnData(_context);

        // fetch data
        var levels = await levelData.GetLevelsAsync(entity.Id);
        var grids = await gridData.GetGridsAsync(entity.Id);
        var materials = await materialData.GetMaterialsAsync(entity.Id);
        var frameProperties = await framePropertiesData.GetFramePropertiesAsync(entity.Id);
        var beams = await beamData.GetBeamsAsync(entity.Id);
        var columns = await columnData.GetColumnsAsync(entity.Id);

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
                FrameProperties = frameProperties
            },
            Elements = new ElementContainer
            {
                Beams = beams,
                Columns = columns
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
        var frameProperties = model.Properties.FrameProperties;
        var beams = model.Elements.Beams;
        var columns = model.Elements.Columns;

        // instantiate data classes
        var levelData = new LevelData(_context);
        var gridData = new GridData(_context);
        var materialData = new MaterialData(_context);
        var framePropertiesData = new FramePropertiesData(_context);
        var beamData = new BeamData(_context);
        var columnData = new ColumnData(_context);

        // save data to context
        levelData.SaveLevels(model.Id, levels);
        gridData.SaveGrids(model.Id, grids);
        materialData.SaveMaterials(model.Id, materials);
        framePropertiesData.SaveFrameProperties(model.Id, frameProperties);
        beamData.SaveBeams(model.Id, beams);
        columnData.SaveColumns(model.Id, columns);
    }

    private void RemoveModelEntities(string modelId)
    {
        var levels = _context.Levels.Where(x => x.ModelId == modelId);
        var grids = _context.Grids.Where(x => x.ModelId == modelId);
        var materials = _context.Materials.Where(x => x.ModelId == modelId);
        var frameProperties = _context.FrameProperties.Where(x => x.ModelId == modelId);
        var beams = _context.Beams.Where(x => x.ModelId == modelId);
        var columns = _context.Columns.Where(x => x.ModelId == modelId);

        _context.Levels.RemoveRange(levels);
        _context.Grids.RemoveRange(grids);
        _context.Materials.RemoveRange(materials);
        _context.FrameProperties.RemoveRange(frameProperties);
        _context.Beams.RemoveRange(beams);
        _context.Columns.RemoveRange(columns);
    }
}
