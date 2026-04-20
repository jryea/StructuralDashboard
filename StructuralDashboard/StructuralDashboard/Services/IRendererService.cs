using StructuralDashboard.Web.Models;

namespace StructuralDashboard.Web.Services;

public interface IRendererService
{
    Task InitializeAsync(string elementId, DotNetObjectReference<object> dotNetRef);
    Task RenderAsync(PlanViewCanvas model);
    Task DestroyAsync();
}
