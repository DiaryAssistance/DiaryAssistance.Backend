using DayOfWeek = DiaryAssistance.Core.Enums.DayOfWeek;

namespace DiaryAssistance.Core.Entities;

public class Schedule
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public Guid SubjectId { get; set; }

    public Guid? TeacherId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Classroom { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Group Group { get; set; } = null!;

    public Subject Subject { get; set; } = null!;

    public User? Teacher { get; set; }
}
