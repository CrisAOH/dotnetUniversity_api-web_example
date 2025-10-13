using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;

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