using Microsoft.AspNetCore.Identity;

namespace DiaryAssistance.Core.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}