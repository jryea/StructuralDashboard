using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Services.Loading;

public sealed class TributaryCalculator : ITributaryCalculator
{
    public IReadOnlyList<LinearTributary> CalculateForBeam(LoadableBeam beam, StructuralModel model)
    {
        // TODO (agent): PORT FROM SizeHeaders.
        //
        // Algorithm:
        //   1. Determine the beam's level and its 2D plan-view geometry (start/end XY).
        //   2. Find all parallel linear framing on the same level:
        //        - other beams with parallel direction vectors
        //        - walls with parallel direction vectors
        //      "Parallel" means direction vectors within some angular tolerance.
        //   3. Project the beam and each candidate neighbor onto a shared axis.
        //   4. For each side of the beam (perpendicular), find the closest
        //      parallel neighbor that overlaps the beam's projected extent.
        //   5. Tributary width on that side = (perpendicular distance) / 2.
        //   6. Identify the floor(s) whose plan polygon overlaps the beam's
        //      tributary strip. Pull DL/LL from the floor's SurfaceLoad.
        //   7. Return one LinearTributary per (side × surface load) combination.
        //      Often there is one per side, sometimes two if a beam spans multiple
        //      surface load regions.
        //
        // EDGE CASES TO HANDLE:
        //   - No parallel framing on one side → tributary width = 0 on that side.
        //     Flag this in the LoadableBeam.Flags (handled at calling site).
        //   - Beam at the edge of a floor (cantilever scenario): out of scope v1,
        //     skip — only handle interior framing.
        //   - Multiple surface loads on one side: split into separate tributaries.
        //
        // REFERENCE: this is the equivalent of SizeHeaders'
        //   - BeamUtils.CalculateTributaryWidth
        //   - WallElement parallel detection
        //   - SurfaceLoad lookup
        // Adapt those algorithms to the StructuralModel input shape.
        throw new NotImplementedException();
    }
}