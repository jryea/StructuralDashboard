using StructuralDashboard.Web.Models;

namespace StructuralDashboard.Web.Rendering;

public interface IStructuralRenderer
{
    Task InitializeAsync(string elementId, DotNetObjectReference<object> dotNetRef);
    Task RenderAsync(PlanViewModel model);
    Task DestroyAsync();
}
