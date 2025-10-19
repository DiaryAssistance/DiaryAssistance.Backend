using DiaryAssistance.Core.Entities;
using DiaryAssistance.Core.Exceptions;
using DiaryAssistance.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DiaryAssistance.Application.Auth.SignOut;

public record SignOutCommand(string? RefreshToken) : IRequest<Unit>;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;

    public SignOutCommandHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null)
            throw new InvalidOperationException("HttpContext is null");

        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        if (user is null)
            throw new BrokenRulesException("Called from unauthorized context");

        int deleted;

        if (request.RefreshToken is null)
            deleted = await _dbContext.RefreshTokens.Where(t => t.UserId == user.Id).ExecuteDeleteAsync(cancellationToken);
        else
            deleted = await _dbContext.RefreshTokens.Where(t => t.UserId == user.Id && t.Token == request.RefreshToken).ExecuteDeleteAsync(cancellationToken);

        return deleted == 0 ? throw new InvalidOperationException("RefreshToken not found") : Unit.Value;
    }
}