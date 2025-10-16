using System.IO;
using Application;
using Application.Interfaces;
using Infrastructure.Fotos;
using Infrastructure.Reports;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.Models;
using WebApiTest.Extensions;
using WebApiTest.Middleware;

var builder = WebApplication.CreateBuilder(args);

var uploadsPath = Path.Combine(builder.Environment.WebRootPath ??
                               Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddPoliciesServices();

//CUANDO NO SON GENÉRICOS:
//builder.Services.AddScoped<IReportService, ReportService>();
//Prueba Commit
//Prueba Commit 2

builder.Services.AddScoped(typeof(IReportService<>), typeof(ReportService<>));
builder.Services.AddSingleton<IFotoService>(new FotoService(uploadsPath));
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseAuthentication();
app.UseAuthorization();

await app.SeedDataAuthentication();
//PARA QUE FUNCIONEN LAS URIs
app.MapControllers();

app.Run();