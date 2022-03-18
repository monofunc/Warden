namespace Warden.Models;

public class ScheduleEntry
{
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public User User { get; set; }
}
