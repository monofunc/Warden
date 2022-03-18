using Microsoft.AspNetCore.Mvc;
using Warden.Dtos;
using Warden.Models;
using Warden.Services;

namespace Warden.Controllers;

[ApiController]
[Route("schedule")]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleService _schedule;

    public ScheduleController(ScheduleService schedule)
    {
        _schedule = schedule;
    }

    [HttpGet("current")]
    public async Task<User> GetCurrentUserAsync()
    {
        return await _schedule.GetCurrentUserAsync();
    }

    [HttpGet]
    public async Task<List<ScheduleEntry>> GetRangeAsync(DateTime? from, DateTime? to)
    {
        var fromDate = from ?? DateTime.Today;
        var toDate = to ?? DateTime.MaxValue;

        return await _schedule.GetRangeAsync(fromDate, toDate);
    }

    [HttpPost]
    public async Task<ScheduleEntry> AddAsync(AddScheduleEntryDto entry)
    {
        return await _schedule.AddAsync(entry.Map());
    }

    [HttpPatch]
    public async Task<List<ScheduleEntry>> ChangeOrderAsync(int first, int second)
    {
        return await _schedule.ChangeOrderAsync(first, second);
    }
}
