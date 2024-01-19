using Api.Infrastructure.ExternalApi;
using Carter;
using Refit;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddAutoMapper(assembly);
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(assembly));

builder.Services.AddCarter();

builder.Services.AddRefitClient<IHackerNewsApi>().ConfigureHttpClient(
    c => c.BaseAddress = new Uri(builder.Configuration["HackerNewsBaseApi"]!));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseAuthorization();

app.MapControllers();

app.Run();