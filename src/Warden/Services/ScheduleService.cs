using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Exceptions;
using Warden.Models;

namespace Warden.Services;

public class ScheduleService
{
    private readonly WardenContext _context;
    private readonly UserService _userService;

    public ScheduleService(WardenContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<User> GetCurrentUserAsync()
    {
        var user = await _context.ScheduleEntries.AsNoTracking()
            .Where(x => x.Date.Date == DateTime.Today)
            .Select(x => x.User)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new ScheduleEntryNotFoundException();
        }

        return user;
    }

    public async Task<List<ScheduleEntry>> GetRangeAsync(DateTime from, DateTime to)
    {
        return await _context.ScheduleEntries.AsNoTracking()
            .Where(x => x.Date.Date >= from.Date && x.Date.Date <= to.Date)
            .Include(x => x.User)
            .ToListAsync();
    }

    public async Task<ScheduleEntry> AddAsync(ScheduleEntry entry)
    {
        var existing = await _context.ScheduleEntries.AsNoTracking()
            .Where(x => x.Date.Date == entry.Date.Date)
            .FirstOrDefaultAsync();

        if (existing is not null)
        {
            throw new ScheduleEntryAlreadyTakenException();
        }

        entry.User = await _userService.GetUserAsync(entry.User.Id);

        await _context.ScheduleEntries.AddAsync(entry);

        await _context.SaveChangesAsync();

        return entry;
    }

    public async Task<List<ScheduleEntry>> ChangeOrderAsync(int first, int second)
    {
        if (first == second)
        {
            throw new ArgumentException("First and second argument cannot be the same");
        }

        var entries = await _context.ScheduleEntries
            .Where(x => x.Id == first || x.Id == second)
            .Include(x => x.User)
            .ToListAsync();

        if (entries.Count != 2)
        {
            // For more granular information, switch to two database round trips
            throw new ScheduleEntryNotFoundException();
        }

        (entries[0].Date, entries[1].Date) = (entries[1].Date, entries[0].Date);

        await _context.SaveChangesAsync();

        return entries;
    }
}
