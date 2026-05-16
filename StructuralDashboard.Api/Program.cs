using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StructuralDashboard.Api.Endpoints;
using StructuralDashboard.Api.Services.Loading;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AddDbContext automatically uses Scoped - per http request
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IStructuralModelRepository, StructuralModelRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IStructuralModelService, StructuralModelService>();
builder.Services.AddScoped<IStructuralGraphService, StructuralGraphService>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ILoadingBuilder, LoadingBuilder>();
builder.Services.AddSingleton<ITributaryCalculator, TributaryCalculator>();
builder.Services.AddSingleton<ILoadAccumulator, LoadAccumulator>();
// Scoped because it depends on scoped IStructuralModelService / IStructuralGraphService.
// IMemoryCache (singleton) carries the actual cached results across scopes.
builder.Services.AddScoped<IMemberLoadingService, MemberLoadingService>();

var app = builder.Build();

app.UseCors("AllowReactDev");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.EnableTryItOutByDefault());
}

// Add exception handler middleware
// Add Authentication and Authorization middleware
// Add logging Middlware

// Syntax for custom middleware
app.Use(async (context, next) =>
{
    // Code Before
    Console.WriteLine(context.Request.Path);
    // next.Invoke() is used to call the next middleware in the pipeline. If you don't call it, the request will end here and won't reach the next middleware or endpoint.
    await next.Invoke();
    // Code After   
    Console.WriteLine(context.Response.StatusCode);
});

// Possible use cases for custom middleware:
// 1. Global error handling
// 2. Request/Response logging
// 3. Timing/Performance 
// 4. Request Transformation
// 5. Rate limiting

app.MapProjectEndpoints();
app.MapStructuralModelEndpoints();
app.MapGraphEndpoints();
app.MapLoadingEndpoints();

app.Run();