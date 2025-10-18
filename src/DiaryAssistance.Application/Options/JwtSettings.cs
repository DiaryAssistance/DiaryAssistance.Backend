namespace DiaryAssistance.Application.Options;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Key { get; set; }
    public TimeSpan Ttl { get; set; }
}