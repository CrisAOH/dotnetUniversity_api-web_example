using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Persistence.Models;

namespace Persistence
{
    public static class SeedDatabase
    {
        public static async Task SeedRolesAndUsersAsync(DbContext context, ILogger? logger, CancellationToken cancellationToken)
        {
            try
            {
                UserManager<AppUser> userManager = context.GetService<UserManager<AppUser>>();
                RoleManager<IdentityRole> roleManager = context.GetService<RoleManager<IdentityRole>>();


                if (userManager.Users.Any())
                {
                    return;
                }

                var adminID = "41282dc4-7533-479a-8b39-9ef338c93a87";
                var clientID = "f5186d9b-5c34-46b8-9b12-4bbd9645407c";

                var roleAdmin = new IdentityRole
                {
                    Id = adminID,
                    Name = CustomRoles.ADMIN,
                    NormalizedName = CustomRoles.ADMIN.ToUpperInvariant(),
                };

                var roleClient = new IdentityRole
                {
                    Id = clientID,
                    Name = CustomRoles.CLIENT,
                    NormalizedName = CustomRoles.CLIENT.ToUpperInvariant(),
                };

                if (!await roleManager.RoleExistsAsync(CustomRoles.ADMIN))
                {
                    await roleManager.CreateAsync(roleAdmin);
                }

                if (!await roleManager.RoleExistsAsync(CustomRoles.CLIENT))
                {
                    await roleManager.CreateAsync(roleClient);
                }

                var userAdmin = new AppUser
                {
                    NombreCompleto = "Cristhian Ortega",
                    UserName = "CAOH",
                    Email = "cristhian@hotmail.com",
                };

                await userManager.CreateAsync(userAdmin, "Password123$"); //SI SE USA ESTA LIBRERÍA DE IDENTITY, LAS CONTRASEÑAS DEBEN TENER MAYUSCULAS, MINUSCULAS, NUMEROS, UN CARACTER ESPECIAL Y UNA LONGITUD.

                var userClient = new AppUser
                {
                    NombreCompleto = "Juan Pérez",
                    UserName = "juanperez",
                    Email = "juan.perez@hotmail.com",
                };

                await userManager.CreateAsync(userClient, "Password123$");

                //AGREGAR ROL A CADA USUARIO
                await userManager.AddToRoleAsync(userAdmin, CustomRoles.ADMIN);
                await userManager.AddToRoleAsync(userClient, CustomRoles.CLIENT);

                //AGREGANDO CLAIMS A ROLES
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.CURSO_READ));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.CURSO_UPDATE));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.CURSO_WRITE));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.CURSO_DELETE));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.INSTRUCTOR_READ));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.INSTRUCTOR_UPDATE));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.INSTRUCTOR_CREATE));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.COMENTARIO_READ));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.COMENTARIO_DELETE));
                await roleManager.AddClaimAsync(roleAdmin, new Claim(CustomClaims.POLICIES, PolicyMaster.COMENTARIO_CREATE));

                await roleManager.AddClaimAsync(roleClient, new Claim(CustomClaims.POLICIES, PolicyMaster.COMENTARIO_READ));
                await roleManager.AddClaimAsync(roleClient, new Claim(CustomClaims.POLICIES, PolicyMaster.INSTRUCTOR_READ));
                await roleManager.AddClaimAsync(roleClient, new Claim(CustomClaims.POLICIES, PolicyMaster.CURSO_READ));
                await roleManager.AddClaimAsync(roleClient, new Claim(CustomClaims.POLICIES, PolicyMaster.COMENTARIO_CREATE));
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Falló el proceso de Identity seed.");
            }
        }

        public static async Task SeedPreciosAsync(ApisWebDbContext dbContext, ILogger? logger, CancellationToken cancelationToken)
        {
            try
            {
                if (dbContext.Precios is null || dbContext.Precios.Any())
                {
                    return;
                }

                string jsonString = GetJsonFile("precios.json");

                if (jsonString is null)
                {
                    return;
                }

                var precios = JsonConvert.DeserializeObject<List<Precio>>(jsonString);

                if (precios is null || precios.Any() == false)
                {
                    return;
                }

                dbContext.Precios.AddRange(precios!);
                await dbContext.SaveChangesAsync(cancelationToken);
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Fallo cargando la data de precios.");
            }
        }

        public static async Task SeedInstructoresAsync(ApisWebDbContext dbContext, ILogger? logger, CancellationToken cancelationToken)
        {
            try
            {
                if (dbContext.Instructores is null || dbContext.Instructores.Any())
                {
                    return;
                }

                string jsonString = GetJsonFile("instructores.json");

                if (jsonString is null)
                {
                    return;
                }

                var instructores = JsonConvert.DeserializeObject<List<Instructor>>(jsonString);

                if (instructores is null || instructores.Any() == false)
                {
                    return;
                }

                dbContext.Instructores.AddRange(instructores!);
                await dbContext.SaveChangesAsync(cancelationToken);
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Fallo cargando la data de instructores.");
            }
        }

        public static async Task SeedCursosAsync(ApisWebDbContext dbContext, ILogger? logger, CancellationToken cancelationToken)
        {
            try
            {
                if (dbContext.Cursos is null || dbContext.Cursos.Any())
                {
                    return;
                }

                string jsonString = GetJsonFile("cursos.json");

                if (jsonString is null)
                {
                    return;
                }

                var instructores = dbContext.Instructores!
                    .ToFrozenDictionary(p => p.ID, p => p); //FROZEN DICTIONARY SIRVE PARA CUANDO LAS LISTAS SÓLO SERÁN UTILIZADAS PARA BÚSQUEDAS

                var precios = dbContext.Precios!.ToFrozenDictionary(p => p.ID, p => p);

                JArray arrayCursos = JArray.Parse(jsonString);

                List<Curso> cursosDB = new List<Curso>();

                foreach (JToken obj in arrayCursos)
                {
                    string? idString = obj["Id"]?.ToString();
                    if (!Guid.TryParse(idString, out Guid id))
                    {
                        id = Guid.NewGuid();
                    }

                    string? titulo = obj["Titulo"]?.ToString();
                    string? descripcion = obj["Descripcion"]?.ToString();
                    DateTime? fechaPublicacion = null;
                    string? fechaPublicacionStr = obj["FechaPublicacion"]?.ToString();

                    if (!string.IsNullOrWhiteSpace(fechaPublicacionStr) && DateTime.TryParse(fechaPublicacionStr, out DateTime fp))
                    {
                        fechaPublicacion = fp;
                    }

                    var curso = new Curso
                    {
                        ID = id,
                        Titulo = titulo,
                        Descripcion = descripcion,
                        FechaPublicacion = fechaPublicacion,
                        Calificaciones = new List<Calificacion>(),
                        Precios = new List<Precio>(),
                        CursoPrecios = new List<CursoPrecio>(),
                        Instructores = new List<Instructor>(),
                        CursoInstructores = new List<CursoInstructor>(),
                        Fotos = new List<Foto>()
                    };

                    if (obj["Precios"] is JArray preciosC)
                    {
                        foreach (JToken pid in preciosC)
                        {
                            Guid idt = new Guid(pid?.ToString()!);
                            if (precios.TryGetValue(idt, out Precio? precio))
                            {
                                curso.Precios.Add(precio);
                            }
                        }
                    }

                    if (obj["Instructores"] is JArray instructoresC)
                    {
                        foreach (JToken iid in instructoresC)
                        {
                            Guid idt = new Guid(iid?.ToString()!);
                            if (instructores.TryGetValue(idt, out Instructor? instructor))
                            {
                                curso.Instructores.Add(instructor);
                            }
                        }
                    }

                    cursosDB.Add(curso);
                }

                await dbContext.Cursos.AddRangeAsync(cursosDB);
                await dbContext.SaveChangesAsync(cancelationToken);
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Fallo cargando la data de cursos.");
            }
        }

        public static async Task SeedCalificacionesAsync(ApisWebDbContext dbContext, ILogger? logger, CancellationToken cancelationToken)
        {
            try
            {
                if (dbContext.Calificaciones is null || dbContext.Calificaciones.Any())
                {
                    return;
                }

                string jsonString = GetJsonFile("calificaciones.json");

                if (jsonString is null)
                {
                    return;
                }

                var calificaciones = JsonConvert.DeserializeObject<List<Calificacion>>(jsonString);

                if (calificaciones is null || calificaciones.Any() == false)
                {
                    return;
                }

                foreach (Calificacion ca in calificaciones!)
                {
                    ca.Curso = null;
                }

                dbContext.Calificaciones.AddRange(calificaciones!);
                await dbContext.SaveChangesAsync(cancelationToken);
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Fallo cargando la data de calificaciones.");
            }
        }

        private static string GetJsonFile(string fileName)
        {
            var leerForma1 = Path.Combine(Directory.GetCurrentDirectory(), "Persistence", "SeedData", fileName);

            var leerForma2 = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", fileName);

            var leerForma3 = Path.Combine(AppContext.BaseDirectory, "SeedData", fileName);

            if (File.Exists(leerForma1))
            {
                return File.ReadAllText(leerForma1);
            }

            if (File.Exists(leerForma2))
            {
                return File.ReadAllText(leerForma2);
            }

            if (File.Exists(leerForma3))
            {
                return File.ReadAllText(leerForma3);
            }

            return null!;
        }
    }
}