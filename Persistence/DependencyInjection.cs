using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApisWebDbContext>(opt =>
            {
                opt.LogTo(Console.WriteLine, new[]
                {
                    DbLoggerCategory.Database.Command.Name
                }, LogLevel.Information).EnableSensitiveDataLogging();

                opt
                .UseSqlite(configuration.GetConnectionString("SQLite"))

                //COMO EL NOMBRE INDICA, ESTO ES PARA LLENAR LAS TABLAS DE FORMA ASÍNCRONA. HAY QUE TENER CUIDADO CON VALORES CAMBIANTES PORQUE ESTO PUEDE CAUSAR ERROR, SE NECESITAN VALORES CONSTANTES
                .UseAsyncSeeding(async (context, status, cancellationToken) =>
                {
                    ApisWebDbContext apiWebDbContext = (ApisWebDbContext)context;
                    var logger = context.GetService<ILogger<ApisWebDbContext>>();

                    try
                    {
                        await SeedDatabase.SeedRolesAndUsersAsync(context, logger, cancellationToken);
                        await SeedDatabase.SeedPreciosAsync(apiWebDbContext, logger, cancellationToken);
                        await SeedDatabase.SeedInstructoresAsync(apiWebDbContext, logger, cancellationToken);
                        await SeedDatabase.SeedCursosAsync(apiWebDbContext, logger, cancellationToken);
                        await SeedDatabase.SeedCalificacionesAsync(apiWebDbContext, logger, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Error en el seeding");
                    }

                });
            });

            return services;
        }
    }
}