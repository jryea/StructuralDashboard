using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Services.Loading;

public interface ITributaryCalculator
{
    /// <summary>
    /// Computes the LinearTributary regions for a single beam, based on the
    /// nearest parallel framing (other beams, walls) on each side and the
    /// floor surface loads that apply.
    /// </summary>
    IReadOnlyList<LinearTributary> CalculateForBeam(LoadableBeam beam, StructuralModel model);
}