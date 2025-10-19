using DiaryAssistance.Application.Auth.Models;
using DiaryAssistance.Application.Options;
using DiaryAssistance.Application.Services;
using DiaryAssistance.Core.Entities;
using DiaryAssistance.Core.Exceptions;
using DiaryAssistance.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DiaryAssistance.Application.Auth.Login;

public record LoginCommand(string Username, string Password) : IRequest<TokensResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokensResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly AppDbContext _dbContext;

    public LoginCommandHandler(UserManager<User> userManager, ITokenGenerator tokenGenerator, AppDbContext dbContext)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
        _dbContext = dbContext;
    }

    public async Task<TokensResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new BrokenRulesException("Failed to authenticate user");

        var accessToken = await _tokenGenerator.GenerateAccessToken(user);
        var refreshToken = new RefreshToken
        {
            UserId = user.Id, Token = _tokenGenerator.GenerateRefreshToken(), Expires = DateTime.UtcNow.AddDays(JwtSettings.RefreshTokenExpirationDays)
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokensResponse(accessToken, refreshToken.Token);
    }
}