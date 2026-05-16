using System;
using System.Collections.Generic;

namespace StructuralDashboard.Api.Domain.Loading;

public sealed record ModelLoadingResult
{
    public required string ModelId { get; init; }
    public required DateTime ComputedAt { get; init; }
    public required IReadOnlyDictionary<string, MemberLoadingResult> Members { get; init; }
    public required IReadOnlyDictionary<string, IReadOnlyList<Reaction>> ReactionsByNode { get; init; }
    public IReadOnlyList<string> Diagnostics { get; init; } = [];
}