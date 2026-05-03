using StructuralDashboard.Web.Models;

namespace StructuralDashboard.Web.Services;

public interface IBeamMatchService
{
    List<BeamMatch> Match(
        PlanViewModel revit,
        PlanViewModel analysis,
        AlignmentTransform analysisTransform,
        double tolerance);
}
