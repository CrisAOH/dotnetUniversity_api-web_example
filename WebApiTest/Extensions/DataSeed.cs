using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using Persistence.Models;

namespace WebApiTest.Extensions
{
    public static class DataSeed
    {
        public static async Task SeedDataAuthentication(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                IServiceProvider service = scope.ServiceProvider;
                ILoggerFactory loggerFactory = service.GetRequiredService<ILoggerFactory>();

                try
                {
                    ApisWebDbContext context = service.GetRequiredService<ApisWebDbContext>();
                    await context.Database.MigrateAsync();
                    /*var userManager = service.GetRequiredService<UserManager<AppUser>>();

                    if (!userManager.Users.Any())
                    {
                        var userAdmin = new AppUser
                        {
                            NombreCompleto = "Cristhian Ortega",
                            UserName = "CrisAOH",
                            Email = "cristhian@hotmail.com",
                        };

                        await userManager.CreateAsync(userAdmin, "Password123$");
                        await userManager.AddToRoleAsync(userAdmin, CustomRoles.ADMIN);

                        var userClient = new AppUser
                        {
                            NombreCompleto = "Juan Perez",
                            UserName = "juanperez",
                            Email = "juan.perez@hotmail.com",
                        };

                        await userManager.CreateAsync(userClient, "Password123$");
                        await userManager.AddToRoleAsync(userClient, CustomRoles.CLIENT);
                    }

                    var cursos = await context.Cursos!.Take(10).Skip(0).ToListAsync();

                    if (!context.Set<CursoInstructor>().Any())
                    {
                        var instructores =
                        await context.Instructores!.Take(10).Skip(0).ToListAsync();

                        foreach (var curso in cursos)
                        {
                            curso.Instructores = instructores;
                        }
                    }

                    if (!context.Set<CursoPrecio>().Any())
                    {
                        var precios = await context.Precios!.ToListAsync();
                        foreach (var curso in cursos)
                        {
                            curso.Precios = precios;
                        }
                    }

                    if (!context.Set<Calificacion>().Any())
                    {
                        foreach (var curso in cursos)
                        {
                            var fakerCalificacion = new Faker<Calificacion>()
                                .RuleFor(c => c.ID, _ => Guid.NewGuid())
                                .RuleFor(c => c.Alumno, f => f.Name.FullName())
                                .RuleFor(c => c.Comentario, f => f.Commerce.ProductDescription())
                                .RuleFor(c => c.Puntaje, 5)
                                .RuleFor(c => c.CursoID, curso.ID);

                            var calificaciones = fakerCalificacion.Generate(10);
                            context.AddRange(calificaciones);
                        }
                    }


                    await context.SaveChangesAsync();*/

                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<ApisWebDbContext>();
                    logger.LogError(e.Message);
                }
            }
        }
    }
}