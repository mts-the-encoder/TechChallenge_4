using Domain.Entities;
using Infrastructure.RepositoryAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests;

public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private User _user;
    private string _password;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(AppDbContext));

                if (descriptor != null) services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase("InMemoryDbForTesting");
                    opt.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedService = scope.ServiceProvider;

                var database = scopedService.GetRequiredService<AppDbContext>();

                database.Database.EnsureDeleted();

                (_user, _password) = ContextSeedInMemory.Seed(database);
            });
    }

    public User GetUser() => _user;

    public string GetPassword() => _password;
}