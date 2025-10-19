using Microsoft.AspNetCore.Identity;

namespace DiaryAssistance.Core.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
}