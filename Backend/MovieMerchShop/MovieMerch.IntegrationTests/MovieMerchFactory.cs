using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using MovieMerchShop.Data;
using MovieMerchShop.Model;

namespace MovieMerch.IntegrationTests;

public class MovieMerchFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var usersContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<UsersContext>));

            if (usersContextDescriptor != null)
            {
                services.Remove(usersContextDescriptor);
            }

            var appDbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (appDbContextDescriptor != null)
            {
                services.Remove(appDbContextDescriptor);
            }

            services.AddDbContext<UsersContext>(options => { options.UseInMemoryDatabase("TestDb"); });
            services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedService = scope.ServiceProvider;
                var usersContext = scopedService.GetRequiredService<UsersContext>();

                usersContext.Database.EnsureDeleted();
                usersContext.Database.EnsureCreated();

                try
                {
                    Console.WriteLine("seeding...");
                    usersContext.Users.Add(new ApplicationUser
                    {
                        Email = "user2@gmail.com",
                        UserName = "tester",
                        Address = "testAddress",
                        BirthDate = new DateTime(1989, 09, 08)
                    });
                    usersContext.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        });
        //builder.UseEnvironment("Development");
    }
    
}