namespace StructuralDashboard.Web.Services;

public class KonvaRenderer : IRendererService
{
    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;

    public KonvaRenderer(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync(string elementId, DotNetObjectReference<object> dotNetRef)
    {
        _module = await _js.InvokeAsync<IJSObjectReference>("import", "./render-host.js");
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
