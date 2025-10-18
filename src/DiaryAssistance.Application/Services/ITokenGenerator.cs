using DiaryAssistance.Core.Entities;

namespace DiaryAssistance.Application.Services;

public interface ITokenGenerator
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(); 
}