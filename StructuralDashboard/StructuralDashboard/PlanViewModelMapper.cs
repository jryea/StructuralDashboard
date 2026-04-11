namespace StructuralDashboard.Web;

public static class PlanViewModelMapper
{
    public static readonly double OFFSET = 10.0;
    public static readonly double ENDPOINT_OFFSET = 9.0;

    public static PlanViewModel ConvertToPlanViewModel (this StructuralModel structuralModel, string levelId)
    {
        var members = new List<PlanMember>();

        var beams = structuralModel.Elements.Beams.Where(b => b.LevelId == levelId);
        var columns = structuralModel.Elements.Columns.Where(c => c.TopLevelId == levelId);

        var planBeams = beams.Select(b => b.ConvertToPlanMember()).ToList();
        var planColumns = columns.Select(c => c.ConvertToPlanMember()).ToList();

        members.AddRange(planBeams);
        members.AddRange(planColumns);

        if (planBeams == null || planBeams.Count() == 0) return new PlanViewModel() {LevelId = levelId};
        var extents = GetExtents(planBeams);

        return new PlanViewModel()
        {
            LevelId = levelId,
            Members = members,
            MinX = extents.MinX,
            MaxX = extents.MaxX, 
            MinY = extents.MinY,
            MaxY = extents.MaxY,
        };
    }

    public static List<PlanViewModel> ConvertToPlanViewModels (this StructuralModel model)
    {
        var levels = model.ModelLayout.Levels;
        return levels.Select(l => model.ConvertToPlanViewModel(l.Id)).ToList();
    }

    public static PlanMember ConvertToPlanMember (this Beam beam)
    {
        var dx = beam.EndPoint.X - beam.StartPoint.X;
        var dy = beam.EndPoint.Y - beam.StartPoint.Y;
        var length = Math.Sqrt(dx * dx + dy * dy);
        var ux = dx / length;
        var uy = dy / length;

        var labelProps = GetLabelProps(beam);

        var beamType = beam.IsJoist ? "joist" : "beam";

        return new PlanMember()
        {
            Id = beam.Id,
            Type = beamType,
            X1 = beam.StartPoint.X + ux * ENDPOINT_OFFSET,
            Y1 = beam.StartPoint.Y + uy * ENDPOINT_OFFSET,
            X2 = beam.EndPoint.X - ux * ENDPOINT_OFFSET,
            Y2 = beam.EndPoint.Y - uy * ENDPOINT_OFFSET,
            Size = "W12x35",
            TagX = labelProps.TagX,
            TagY = labelProps.TagY,
            TagRotation = labelProps.TagRotation,
        };
    }

    public static PlanMember ConvertToPlanMember(this Column column)
    {

        return new PlanMember()
        {
            Id = column.Id,
            Type = "column",
            X1 = column.StartPoint.X,
            Y1 = column.StartPoint.Y,
            X2 = 0,
            Y2 = 0,
            Orientation = column.Orientation
        };
    }

    private static (double TagX, double TagY, double TagRotation) GetLabelProps(Beam beam)
    {
        // Get Center Point of beam line through endpoints
        var x1 = beam.StartPoint.X;
        var x2 = beam.EndPoint.X;
        var y1 = beam.StartPoint.Y;
        var y2 = beam.EndPoint.Y;

        var midX = (x1 + x2) / 2;
        var midY = (y1 + y2) / 2;

        var dx = x2 - x1;
        var dy = y2 - y1;

        var length = Math.Sqrt(dx * dx + dy * dy);
        var px = -dy / length;
        var py = dx / length;
        var offsetX = midX + px * OFFSET;
        var offsetY = midY + py * OFFSET;

        var radians = Math.Atan2(y2 - y1, x2 - x1);
        var degrees = radians * (180.0 / Math.PI);

        return (offsetX, offsetY, degrees);
    }

    private static (double MinX, double MaxX, double MinY, double MaxY) GetExtents(List<PlanMember> members)
    {
        var xValues = new List<double>();
        var yValues = new List<double>();

        foreach (var m in members)
        {
            xValues.Add(m.X1);
            xValues.Add(m.X2);
            yValues.Add(m.Y1);
            yValues.Add(m.Y2);
        }

        return (xValues.Min(), xValues.Max(), yValues.Min(), yValues.Max());
    }
}
