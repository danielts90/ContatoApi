using ContatoApi.Configurations;
using ContatoApi.Context;
using ContatoApi.Entities;
using ContatoApi.Models;
using ContatoApi.RabbitMq;
using ContatoApi.Services;
using Microsoft.EntityFrameworkCore;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContatoDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("ddd", httpclient => {
    httpclient.BaseAddress = new Uri("http://localhost:5076/");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Contato API";
    config.Title = "ContaotApi v1";
    config.Version = "v1";
});

builder.Services.AddHostedService<DddHostedService>();
builder.Services.AddSingleton<IDddService, DddService>();

var app = builder.Build();

app.UsePrometheusMetrics();

var consumer = new RabbitMqConsumer<Ddd>("localhost", "ddd.updated", app.Services.GetRequiredService<IDddService>());
Task.Run(() => consumer.StartConsumer());

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "ContatoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ContatoDb>();
}

app.MapContatoEndpoints();
app.MapGet("/ddds", (IDddService dddService) =>
{
    return TypedResults.Ok(dddService.GetCachedDdds());
});

app.MapMetrics();
app.Run();

public partial class Program { }
