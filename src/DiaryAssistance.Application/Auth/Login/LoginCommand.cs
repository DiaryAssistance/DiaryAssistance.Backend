using DiaryAssistance.Application.Auth.Models;
using MediatR;

namespace DiaryAssistance.Application.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<TokensResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokensResponse>
{
    public async Task<TokensResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return new TokensResponse("", "");
    }
}

