namespace StructuralDashboard.Api.Services;

public class ProjectService : IProjectService
{
    private readonly HttpClient _httpClient;

    public ProjectService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    public async Task<List<Project>> GetAllAsync()
    {
        var projects = await _httpClient.GetFromJsonAsync<List<Project>>("/api/projects");
        if (projects == null) return new List<Project>();

        return projects;
    }

    public async Task CreateAsync(Project project)
    {
        await _httpClient.PostAsJsonAsync("/api/projects", project);
    }

    public async Task DeleteAsync(string projectNumber)
    {
        await _httpClient.DeleteAsync($"/api/projects/{projectNumber}");
    }


    public async Task UpdateAsync(Project project)
    {
        await _httpClient.PutAsJsonAsync($"/api/projects/{project.ProjectNumber}", project);
    }
}
