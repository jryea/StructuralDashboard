namespace StructuralDashboard.Api.Domain.Loading;

public sealed record LinearTributary
{
    public required double Width { get; init; }
    public required string SurfaceLoadId { get; init; }
    public required double DeadLoadPsf { get; init; }
    public required double LiveLoadPsf { get; init; }
    public double ResultingLinePlf => (DeadLoadPsf + LiveLoadPsf) * Width;
}