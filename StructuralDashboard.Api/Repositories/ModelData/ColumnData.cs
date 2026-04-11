namespace StructuralDashboard.Api.Repositories.ModelData;

public class ColumnData
{
    private readonly AppDbContext _context;

    public ColumnData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Column>> GetColumnsAsync(string modelId)
    {
        var entities = await _context.Columns
            .Where(c => c.ModelId == modelId).ToListAsync();

        var columns = entities.Select(c => new Column
        {
            Id = c.Id,
            StartPoint = c.StartPoint,
            EndPoint = c.EndPoint,
            BaseLevelId = c.BaseLevelId,
            TopLevelId = c.TopLevelId,
            Orientation = c.Orientation,
            FramePropertiesId = c.FramePropertiesId,
            IsLateral = c.IsLateral,
        }).ToList();

        return columns;
    }

    public void SaveColumns(string modelId, List<Column> columns)
    {
        var entities = columns.Select(c => new ColumnEntity
        {
            Id = c.Id,
            ModelId = modelId,
            StartPoint = c.StartPoint,
            EndPoint = c.EndPoint,
            BaseLevelId = c.BaseLevelId,
            TopLevelId = c.TopLevelId,
            Orientation = c.Orientation,
            FramePropertiesId = c.FramePropertiesId,
            IsLateral = c.IsLateral,
        }).ToList();

        _context.Columns.AddRange(entities);
    }
}
