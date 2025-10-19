using FluentValidation;

namespace DiaryAssistance.Application.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty().NotNull();
        RuleFor(c => c.Password).NotEmpty().NotNull().MinimumLength(6);
    }
}