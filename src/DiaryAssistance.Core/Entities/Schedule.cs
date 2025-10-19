namespace DiaryAssistance.Core.Entities;
using DiaryAssistance.Core.Enums;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public User Teacher { get; set; }
    public string SubjectName { get; set; }
    public DayOfWeekEnum DayOfWeek { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string RoomNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}