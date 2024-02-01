using Microsoft.EntityFrameworkCore;
using MovieMerchShop.Data;

namespace MovieMerchShop.Extension;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var usersAppContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
        using var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        try
        {
            if (usersAppContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" &&
                appContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
              usersAppContext.Database.Migrate();
              appContext.Database.Migrate();  
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return webApp;
    }
}