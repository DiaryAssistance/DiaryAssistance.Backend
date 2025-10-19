using FluentValidation;

namespace DiaryAssistance.Application.Auth.SignOut;

public class SignOutCommandValidator : AbstractValidator<SignOutCommand>
{
    public SignOutCommandValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty().When(c => c.RefreshToken is not null);
    }
}