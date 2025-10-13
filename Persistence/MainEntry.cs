// using System;
// using System.Threading;
// using Domain;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Persistence;
// using Persistence.Models;

// ServiceCollection services = new ServiceCollection();
// services.AddLogging(l =>
// {
//     l.ClearProviders();
// });

// services.AddDbContext<ApisWebDbContext>();

// services.AddIdentityCore<AppUser>(options =>
// {
//     options.Password.RequiredLength = 6;
//     options.Password.RequireNonAlphanumeric = false;
//     options.Password.RequireDigit = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequireLowercase = true;
//     options.User.RequireUniqueEmail = true;
// }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApisWebDbContext>();

// ServiceProvider provider = services.BuildServiceProvider();

// try
// {
//     using (IServiceScope scope = provider.CreateScope())
//     {
//         ApisWebDbContext context = scope.ServiceProvider.GetRequiredService<ApisWebDbContext>();
//         await context.Database.MigrateAsync();
//         Console.WriteLine("La migración y el seeding han sido completados.");

//         UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
//         RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//         ILogger logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedIdentity");

//         await SeedDatabase.SeedRolesAndUsersAsync(userManager, roleManager, logger, CancellationToken.None);
//     }
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"Error en el seeding/migration {ex.Message}");
// }