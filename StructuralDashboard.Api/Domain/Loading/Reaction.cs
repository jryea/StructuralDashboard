namespace StructuralDashboard.Api.Domain.Loading;

public sealed record Reaction
{
    public required string FromMemberId { get; init; }
    public required string AtNodeId { get; init; }
    public required double MagnitudeLbs { get; init; }
    public ReactionDirection Direction { get; init; } = ReactionDirection.Down;
}

public enum ReactionDirection
{
    Down,
    Up,
    LateralX,
    LateralY
}