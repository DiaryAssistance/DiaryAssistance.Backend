namespace DiaryAssistance.Core.Entities;

public class Subject
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Code { get; set; }

    public ICollection<Schedule> Schedules { get; set; } = [];
}
