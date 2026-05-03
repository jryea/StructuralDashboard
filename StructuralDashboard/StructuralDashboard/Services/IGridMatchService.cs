using StructuralDashboard.Web.Models;

namespace StructuralDashboard.Web.Services;

public interface IGridMatchService
{
    void Match(
        PlanViewModel revit,
        PlanViewModel analysis,
        AlignmentTransform analysisTransform,
        double tolerance);
}
