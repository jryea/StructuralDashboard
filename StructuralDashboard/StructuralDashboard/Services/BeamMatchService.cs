using StructuralDashboard.Web.Models;

namespace StructuralDashboard.Web.Services;

public class BeamMatchService : IBeamMatchService
{
    public List<BeamMatch> Match(
        PlanViewModel revit,
        PlanViewModel analysis,
        AlignmentTransform analysisTransform,
        double tolerance)
    {
        var revitBeams = FilterBeams(revit);
        var analysisBeams = FilterBeams(analysis);

        ClearStatus(revitBeams);
        ClearStatus(analysisBeams);

        var pairing = MemberMatchMath.PairMembers(revitBeams, analysisBeams, analysisTransform, tolerance);

        var results = new List<BeamMatch>();

        foreach (var (r, a) in pairing.Pairs)
        {
            if (a is null)
            {
                r.MatchStatus = "R Only";
                results.Add(new BeamMatch
                {
                    RevitId = r.Id ?? "-",
                    RevitSize = r.Label ?? "-",
                    AnalysisId = "-",
                    AnalysisSize = "-",
                    Status = "R Only"
                });
            }
            else
            {
                var status = string.Equals(r.Label, a.Label, StringComparison.Ordinal) ? "Match" : "Size";
                r.MatchStatus = status;
                a.MatchStatus = status;
                results.Add(new BeamMatch
                {
                    RevitId = r.Id ?? "-",
                    RevitSize = r.Label ?? "-",
                    AnalysisId = a.Id ?? "-",
                    AnalysisSize = a.Label ?? "-",
                    Status = status
                });
            }
        }

        foreach (var a in pairing.AnalysisOnly)
        {
            a.MatchStatus = "A Only";
            results.Add(new BeamMatch
            {
                RevitId = "-",
                RevitSize = "-",
                AnalysisId = a.Id ?? "-",
                AnalysisSize = a.Label ?? "-",
                Status = "A Only"
            });
        }

        return results;
    }

    private static List<PlanMember> FilterBeams(PlanViewModel model)
    {
        if (model?.Members is null) return new();
        return model.Members
            .Where(m => m.Type == "beam" || m.Type == "joist")
            .ToList();
    }

    private static void ClearStatus(IEnumerable<PlanMember> members)
    {
        foreach (var m in members) m.MatchStatus = null;
    }
}
