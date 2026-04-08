namespace StructuralDashboard.Web.Rendering;

public static class PlanViewModelMapper
{
    public static PlanViewModel ConvertToPlanViewModel (this StructuralModel structuralModel, string levelId)
    {
        var beams = structuralModel.Elements.Beams.Where(b => b.LevelId == levelId);

        var planBeams = beams.Select(b => new PlanMember()
        {
            Id = b.Id,
            Type = "beam",
            X1 = b.StartPoint.X,
            Y1 = b.StartPoint.Y,
            X2 = b.EndPoint.X,
            Y2 = b.EndPoint.Y
        }).ToList();

        var extents = GetExtents(planBeams);

        return new PlanViewModel()
        {
            LevelId = levelId,
            Members = planBeams,
            MinX = extents.MinX,
            MaxX = extents.MaxX,
            MinY = extents.MinY,
            MaxY = extents.MaxY,
        };
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
