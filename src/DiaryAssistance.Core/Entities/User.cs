using Microsoft.AspNetCore.Identity;

namespace DiaryAssistance.Core.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Schedule> Schedules { get; set; }
}