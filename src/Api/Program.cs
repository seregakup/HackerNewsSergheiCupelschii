using System.Reflection;
using Api.ExceptionHandlers;
using Api.Features.BestStories;
using Api.Infrastructure.Cache;
using Api.Infrastructure.ExternalApi;
using Carter;
using Refit;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddAutoMapper(assembly);
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(assembly));

builder.Services.AddCarter();

builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddRefitClient<IHackerNewsApi>().ConfigureHttpClient(
    c => c.BaseAddress = new Uri(builder.Configuration["HackerNewsBaseApi"]!));

builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseAuthorization();

app.MapControllers();

app.Run();