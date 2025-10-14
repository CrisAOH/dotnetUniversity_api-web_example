using Application;
using Application.Interfaces;
using Infrastructure.Reports;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using WebApiTest.Extensions;
using WebApiTest.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

//CUANDO NO SON GENÉRICOS:
//builder.Services.AddScoped<IReportService, ReportService>();
//Prueba Commit
//Prueba Commit 2

builder.Services.AddScoped(typeof(IReportService<>), typeof(ReportService<>));

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//PARA AÑADIR CONTROLADORES


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.SeedDataAuthentication();
//PARA QUE FUNCIONEN LAS URIs
app.MapControllers();

app.Run();