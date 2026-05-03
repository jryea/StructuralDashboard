using StructuralDashboard.Web.Models;

namespace StructuralDashboard.Web.Services;

public class GridMatchService : IGridMatchService
{
    public void Match(
        PlanViewModel revit,
        PlanViewModel analysis,
        AlignmentTransform analysisTransform,
        double tolerance)
    {
        var revitGrids = FilterGrids(revit);
        var analysisGrids = FilterGrids(analysis);

        foreach (var m in revitGrids) m.MatchStatus = null;
        foreach (var m in analysisGrids) m.MatchStatus = null;

        var pairing = MemberMatchMath.PairMembers(revitGrids, analysisGrids, analysisTransform, tolerance);

        foreach (var (r, a) in pairing.Pairs)
        {
            if (a is null)
            {
                r.MatchStatus = "R Only";
            }
            else
            {
                r.MatchStatus = "Match";
                a.MatchStatus = "Match";
            }
        }

        foreach (var a in pairing.AnalysisOnly)
        {
            a.MatchStatus = "A Only";
        }
    }

    private static List<PlanMember> FilterGrids(PlanViewModel model)
    {
        if (model?.Members is null) return new();
        return model.Members.Where(m => m.Type == "grid").ToList();
    }
}
