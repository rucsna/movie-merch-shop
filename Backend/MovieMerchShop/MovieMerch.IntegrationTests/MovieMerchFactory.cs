using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieMerchShop.Data;

namespace MovieMerch.IntegrationTests;

public class MovieMerchFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the production DbContext registration with the in-memory version.
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(UsersContext));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<UsersContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabaseForTesting");
            });
        });

    }
}