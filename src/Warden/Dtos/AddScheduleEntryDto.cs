using Warden.Models;

namespace Warden.Dtos;

public record AddScheduleEntryDto(DateTime Date, int UserId)
{
    // Automapper is overkill for one DTO, right? :D
    public ScheduleEntry Map()
    {
        return new ScheduleEntry
        {
            Date = Date,
            User = new User
            {
                Id = UserId
            }
        };
    }
}
