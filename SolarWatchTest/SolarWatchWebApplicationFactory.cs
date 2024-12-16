using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Data;
using SolarWatch.Services;

namespace SolarWatchTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();
    private static string _accessToken = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));
            
            if (solarWatchDbContextDescriptor != null)
            {
                services.Remove(solarWatchDbContextDescriptor);
            }
            
            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            using var scope = services.BuildServiceProvider().CreateScope();
            
            var scopedServices = scope.ServiceProvider;
         
            var solarContext = scopedServices.GetRequiredService<SolarWatchContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();
            
            var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
            authenticationSeeder.AddRoles();
            authenticationSeeder.AddAdmin();
            
            var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
            var tokenService = scopedServices.GetRequiredService<ITokenService>();
            var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
            SeedUsers(userManager, tokenService, roleManager).Wait();
        });
        
        async Task SeedUsers(UserManager<IdentityUser> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            var user = new IdentityUser { UserName = "testUser", Email = "test@test.com" };
            await userManager.CreateAsync(user, "TestUser123");
            var roles = roleManager.Roles.ToList();
            _accessToken = tokenService.CreateToken(user, roles[0].Name!);
        }
    }
}