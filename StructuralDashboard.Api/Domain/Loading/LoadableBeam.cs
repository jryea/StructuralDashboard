using System.Collections.Generic;

namespace StructuralDashboard.Api.Domain.Loading;

public sealed class LoadableBeam
{
    public required string MemberId { get; init; }
    public required string StartNodeId { get; init; }
    public required string EndNodeId { get; init; }
    public required double Span { get; init; }

    public List<LinearTributary> Tributaries { get; } = [];
    public List<Reaction> IncomingReactions { get; } = [];
    public List<Reaction> OutgoingReactions { get; } = [];

    public double DistributedLoad
    {
        get
        {
            double total = 0;
            foreach (var t in Tributaries)
                total += t.ResultingLinePlf;
            return total;
        }
    }

    public MemberLoadingResult ToResult(IEnumerable<string>? flags = null) =>
        new()
        {
            MemberId = MemberId,
            SpanFt = SpanFt,
            DistributedLoad = DistributedLoad,
            ContributingTributaries = Tributaries.AsReadOnly(),
            IncomingPointLoads = IncomingReactions.AsReadOnly(),
            EndReactions = OutgoingReactions.AsReadOnly(),
            Flags = flags?.ToList() ?? []
        };
}