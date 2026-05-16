using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Api.Repositories;
using StructuralDashboard.Api.Services;
using StructuralDashboard.Api.Tests.Fixtures;
using StructuralDashboard.Shared.Contracts;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace StructuralDashboard.Api.Tests.Integration;

[TestFixture]
public class LoadingEndpointTests
{
    private LoadingTestFactory _factory = null!;
    private HttpClient _client = null!;
    private StructuralModel _model = null!;

    [SetUp]
    public void Setup()
    {
        _model = ModelFixtures.WallSupportedBeam();
        _factory = new LoadingTestFactory(_model);
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetMember_ValidMember_Returns200WithLoadingBody()
    {
        var response = await _client.GetAsync($"/api/loading/{_model.Id}/members/B1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var body = await response.Content.ReadFromJsonAsync<MemberLoadingResult>(opts);

        Assert.That(body, Is.Not.Null);
        Assert.That(body!.MemberId, Is.EqualTo("B1"));
        Assert.That(body.SpanFt, Is.EqualTo(20.0).Within(1e-6));
        Assert.That(body.DistributedLoad, Is.EqualTo(500).Within(1e-3));
        Assert.That(body.ContributingTributaries, Is.Not.Empty);
        Assert.That(body.EndReactions, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetMember_UnknownMember_Returns404()
    {
        var response = await _client.GetAsync($"/api/loading/{_model.Id}/members/does-not-exist");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetMember_UnknownModel_Returns404()
    {
        var response = await _client.GetAsync("/api/loading/MDL-DOES-NOT-EXIST/members/B1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private sealed class LoadingTestFactory : WebApplicationFactory<Program>
    {
        private readonly StructuralModel _model;
        public LoadingTestFactory(StructuralModel model) => _model = model;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services =>
            {
                // Strip the EF/SQL chain so the test host doesn't try to reach SQL Server.
                services.RemoveAll<DbContextOptions<Data.AppDbContext>>();
                services.RemoveAll<Data.AppDbContext>();
                services.RemoveAll<IStructuralModelRepository>();
                services.RemoveAll<IProjectRepository>();
                services.RemoveAll<IProjectService>();
                services.RemoveAll<IStructuralModelService>();
                services.RemoveAll<IStructuralGraphService>();

                services.AddSingleton<IStructuralModelService>(_ => new InMemoryModelService(_model));
                services.AddScoped<IStructuralGraphService, StructuralGraphService>();
                // Empty stub so the (unrelated) project endpoints still bind their service param.
                services.AddSingleton<IProjectService, NoopProjectService>();
            });
        }
    }

    private sealed class NoopProjectService : IProjectService
    {
        public Task<List<Project>> GetAllProjectsAsync() => Task.FromResult(new List<Project>());
        public Task<Project?> GetProjectAsync(string projectNumber) => Task.FromResult<Project?>(null);
        public Task CreateProjectAsync(Project project) => Task.CompletedTask;
        public Task UpdateProjectAsync(Project project) => Task.CompletedTask;
        public Task DeleteProjectAsync(string projectNumber) => Task.CompletedTask;
    }

    private sealed class InMemoryModelService : IStructuralModelService
    {
        private readonly StructuralModel _model;
        public InMemoryModelService(StructuralModel model) => _model = model;
        public Task<StructuralModel?> GetModelAsync(string modelId) =>
            Task.FromResult<StructuralModel?>(modelId == _model.Id ? _model : null);
        public Task<List<StructuralModel>> GetAllModelsAsync(string p) => Task.FromResult(new List<StructuralModel> { _model });
        public Task CreateModelAsync(StructuralModel m, string p) => Task.CompletedTask;
        public Task UpdateModelAsync(StructuralModel m) => Task.CompletedTask;
        public Task DeleteModelAsync(string id) => Task.CompletedTask;
    }
}
