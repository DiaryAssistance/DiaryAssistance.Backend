namespace DiaryAssistance.Core.Entities;

public class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Year { get; set; }

    public string? Description { get; set; }

    public ICollection<User> Students { get; set; } = [];

    public ICollection<Schedule> Schedules { get; set; } = [];
}
