using DiaryAssistance.Application.Auth.Models;
using DiaryAssistance.Core.Entities;
using DiaryAssistance.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DiaryAssistance.Application.Auth.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password, string Group, string? Role) : IRequest<UserResponse>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(AppDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<UserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.Email) is not null)
            throw new InvalidOperationException($"User with email: {request.Email} already exists.");

        var user = new User
        {
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Group = request.Group,
            Email = request.Email
        };

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var registerResult = await _userManager.CreateAsync(user, request.Password);

        if (!registerResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            var errorString = string.Join(',', registerResult.Errors.Select(c => c.Description));
            throw new InvalidOperationException($"Failed to create user. Errors: {errorString}");
        }

        if (request.Role is not null)
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!addToRoleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var errorString = string.Join(',', registerResult.Errors.Select(c => c.Description));
                throw new InvalidOperationException($"Failed to add user to role: {request.Role}. Errors: {errorString}");
            }
        }

        await transaction.CommitAsync(cancellationToken);
        return new UserResponse($"{user.FirstName} {user.LastName}", user.Email, user.Group);
    }
}