using Microsoft.EntityFrameworkCore;
using Warden.Models;

namespace Warden.Data;

public class WardenContext : DbContext
{
    public WardenContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ScheduleEntry> ScheduleEntries => Set<ScheduleEntry>();
    public DbSet<User> Users => Set<User>();
}

public static class Seeder
{
    public static async Task SeedDataAsync(WardenContext context)
    {
        var bob = new User { Name = "Bob" };
        var alice = new User { Name = "Alice" };
        var john = new User { Name = "John" };

        await context.Users.AddRangeAsync(bob, alice, john);

        await context.ScheduleEntries.AddRangeAsync(new List<ScheduleEntry>
        {
            new()
            {
                Date = DateTime.UtcNow.Date,
                User = bob
            },
            new()
            {
                Date = DateTime.UtcNow.Date + TimeSpan.FromDays(1),
                User = alice
            },
            new()
            {
                Date = DateTime.UtcNow.Date + TimeSpan.FromDays(2),
                User = john
            }
        });

        await context.SaveChangesAsync();
    }
}
