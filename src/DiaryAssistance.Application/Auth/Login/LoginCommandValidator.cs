using FluentValidation;

namespace DiaryAssistance.Application.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().NotNull().MinimumLength(6);
    }
}