using FluentValidation;

namespace DiaryAssistance.Application.Auth.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(t => t.RefreshToken).NotEmpty().NotNull();
    }
}