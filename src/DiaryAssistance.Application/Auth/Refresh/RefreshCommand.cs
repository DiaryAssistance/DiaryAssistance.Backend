using DiaryAssistance.Application.Auth.Models;
using DiaryAssistance.Application.Options;
using DiaryAssistance.Application.Services;
using DiaryAssistance.Core.Exceptions;
using DiaryAssistance.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiaryAssistance.Application.Auth.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<TokensResponse>;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, TokensResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ITokenGenerator _tokenGenerator;

    public RefreshCommandHandler(AppDbContext dbContext, ITokenGenerator tokenGenerator)
    {
        _dbContext = dbContext;
        _tokenGenerator = tokenGenerator;
    }
    public async Task<TokensResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _dbContext.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == request.RefreshToken, cancellationToken: cancellationToken);

        if (refreshToken is null)
            throw new NotFoundException("Refresh token not found");

        if (refreshToken.Expires < DateTime.UtcNow)
            throw new BrokenRulesException("The refresh token has expired");

        var accessToken = await _tokenGenerator.GenerateAccessToken(refreshToken.User);
        
        refreshToken.Token = _tokenGenerator.GenerateRefreshToken();
        refreshToken.Expires = DateTime.UtcNow.AddDays(JwtSettings.RefreshTokenExpirationDays);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return new TokensResponse(accessToken, refreshToken.Token);
    }
}