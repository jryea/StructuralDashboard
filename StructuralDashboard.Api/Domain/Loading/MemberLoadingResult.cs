using System.Collections.Generic;

namespace StructuralDashboard.Api.Domain.Loading;

public sealed record MemberLoadingResult
{
    public required string MemberId { get; init; }
    public required double SpanFt { get; init; }
    public required double DistributedLoad { get; init; }
    public IReadOnlyList<LinearTributary> ContributingTributaries { get; init; } = [];
    public IReadOnlyList<Reaction> IncomingPointLoads { get; init; } = [];
    public IReadOnlyList<Reaction> EndReactions { get; init; } = [];
    public IReadOnlyList<string> Flags { get; init; } = [];
}