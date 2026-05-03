using StructuralDashboard.Web.Models;

namespace StructuralDashboard.Web.Services;

internal static class MemberMatchMath
{
    public record MemberPairing(
        List<(PlanMember Revit, PlanMember? Analysis)> Pairs,
        List<PlanMember> AnalysisOnly);

    public static MemberPairing PairMembers(
        IReadOnlyList<PlanMember> revitMembers,
        IReadOnlyList<PlanMember> analysisMembers,
        AlignmentTransform transform,
        double tolerance)
    {
        var transformed = analysisMembers.Select(m => TransformMember(m, transform)).ToList();

        var candidates = new List<(int ri, int ai, double sum)>();
        for (int i = 0; i < revitMembers.Count; i++)
        {
            for (int j = 0; j < transformed.Count; j++)
            {
                var d = EndpointPairDistance(revitMembers[i], transformed[j]);
                if (d.max <= tolerance)
                    candidates.Add((i, j, d.sum));
            }
        }

        candidates.Sort((a, b) => a.sum.CompareTo(b.sum));

        var revMatched = new int?[revitMembers.Count];
        var anaConsumed = new bool[analysisMembers.Count];
        foreach (var (ri, ai, _) in candidates)
        {
            if (revMatched[ri] is not null) continue;
            if (anaConsumed[ai]) continue;
            revMatched[ri] = ai;
            anaConsumed[ai] = true;
        }

        var pairs = new List<(PlanMember, PlanMember?)>(revitMembers.Count);
        for (int i = 0; i < revitMembers.Count; i++)
        {
            PlanMember? match = revMatched[i] is int ai ? analysisMembers[ai] : null;
            pairs.Add((revitMembers[i], match));
        }

        var analysisOnly = new List<PlanMember>();
        for (int j = 0; j < analysisMembers.Count; j++)
        {
            if (!anaConsumed[j]) analysisOnly.Add(analysisMembers[j]);
        }

        return new MemberPairing(pairs, analysisOnly);
    }

    private static TransformedMember TransformMember(PlanMember m, AlignmentTransform t)
    {
        var (x1, y1) = ApplyTransform(m.X1, m.Y1, t);
        var (x2, y2) = ApplyTransform(m.X2, m.Y2, t);
        return new TransformedMember(m, x1, y1, x2, y2);
    }

    private static (double x, double y) ApplyTransform(double x, double y, AlignmentTransform t)
    {
        var rad = t.Rotation * Math.PI / 180.0;
        var cos = Math.Cos(rad);
        var sin = Math.Sin(rad);
        return (x * cos - y * sin + t.X, x * sin + y * cos + t.Y);
    }

    private static (double sum, double max) EndpointPairDistance(PlanMember r, TransformedMember a)
    {
        var d11 = Distance(r.X1, r.Y1, a.X1, a.Y1);
        var d22 = Distance(r.X2, r.Y2, a.X2, a.Y2);
        var d12 = Distance(r.X1, r.Y1, a.X2, a.Y2);
        var d21 = Distance(r.X2, r.Y2, a.X1, a.Y1);

        var forwardSum = d11 + d22;
        var reverseSum = d12 + d21;

        if (forwardSum <= reverseSum)
            return (forwardSum, Math.Max(d11, d22));
        return (reverseSum, Math.Max(d12, d21));
    }

    private static double Distance(double x1, double y1, double x2, double y2)
    {
        var dx = x2 - x1;
        var dy = y2 - y1;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private record TransformedMember(PlanMember Member, double X1, double Y1, double X2, double Y2);
}
