namespace StructuralDashboard.Web.Rendering;

public class ThreeJsRenderer : IStructuralRenderer
{
    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;

    public ThreeJsRenderer(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync(string elementId, DotNetObjectReference<object> dotNetRef)
    {
        //_module = await _js.InvokeAsync<IJSObjectReference>("import", modulePath);
        await _module.InvokeVoidAsync("initialize", elementId, dotNetRef);
    }

    public async Task RenderAsync(PlanViewModel model)
    {
        await _module!.InvokeVoidAsync("render", model);
    }
    public async Task DestroyAsync()
    {
        await _module!.InvokeVoidAsync("destroy");
        await _module.DisposeAsync();
    }
}
