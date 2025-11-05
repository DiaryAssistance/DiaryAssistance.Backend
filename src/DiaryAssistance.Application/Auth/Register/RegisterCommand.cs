using DiaryAssistance.Application.Auth.Models;
using DiaryAssistance.Core.Constants;
using DiaryAssistance.Core.Entities;
using DiaryAssistance.Core.Exceptions;
using DiaryAssistance.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DiaryAssistance.Application.Auth.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string Username,string Password, string? Role) : IRequest<UserResponse>;

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
        if (await _userManager.FindByNameAsync(request.Username) is not null)
            throw new ConflictException($"User with username '{request.Username}' already exists.");

        var user = new User
        {
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var registerResult = await _userManager.CreateAsync(user, request.Password);

        if (!registerResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            var errors = new Dictionary<string, string[]>
            {
                ["Identity"] = registerResult.Errors.Select(e => e.Description).ToArray()
            };
            throw new BadRequestException("Failed to create user.", errors);
        }

        if (request.Role is not null)
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!addToRoleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var errors = new Dictionary<string, string[]>
                {
                    ["Role"] = addToRoleResult.Errors.Select(e => e.Description).ToArray()
                };
                throw new BadRequestException($"Failed to add user to role '{request.Role}'.", errors);
            }
        }
        else
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Student);
            if (!addToRoleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var errors = new Dictionary<string, string[]>
                {
                    ["Role"] = addToRoleResult.Errors.Select(e => e.Description).ToArray()
                };
                throw new BadRequestException($"Failed to add user to role '{Roles.Student}'.", errors);
            }
        }

        await transaction.CommitAsync(cancellationToken);
        return new UserResponse($"{user.FirstName} {user.LastName}", user.UserName,user.Email);
    }
}