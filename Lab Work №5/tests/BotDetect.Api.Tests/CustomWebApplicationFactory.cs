using System.Net.Http;
using BotDetect.Domain.Entities;
using BotDetect.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BotDetect.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private bool _seeded;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BotDetectDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<BotDetectDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        });
    }

    public override HttpClient CreateClient(WebApplicationFactoryClientOptions options)
    {
        var client = base.CreateClient(options);
        if (!_seeded)
        {
            SeedOnce(Server.Services);
            _seeded = true;
        }
        return client;
    }

    private static void SeedOnce(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BotDetectDbContext>();
        db.Database.EnsureCreated();
        if (db.Users.Any()) return;
        Seed(db);
    }

    private static void Seed(BotDetectDbContext db)
    {

        db.Users.Add(new User
        {
            UserId = 1,
            Username = "testuser",
            Email = "test@example.com",
            Password = "test123"
        });

        foreach (var r in new[]
                 {
                     (1, "Неизвестно", 0),
                     (2, "Низкая вероятность бота", 1),
                     (3, "Средняя вероятность бота", 2),
                     (4, "Высокая вероятность бота", 3),
                     (5, "Подтверждённый бот", 4)
                 })
            db.BotRatings.Add(new BotRating
            {
                RatingId = r.Item1,
                Description = r.Item2,
                RatingValue = r.Item3
            });

        db.SaveChanges();
    }
}
