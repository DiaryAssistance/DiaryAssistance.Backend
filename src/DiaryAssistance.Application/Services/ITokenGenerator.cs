using DiaryAssistance.Core.Entities;

namespace DiaryAssistance.Application.Services;

public interface ITokenGenerator
{
    Task<string> GenerateAccessToken(User user);
    string GenerateRefreshToken(); 
}