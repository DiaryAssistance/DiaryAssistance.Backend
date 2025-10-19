using FluentValidation;

namespace DiaryAssistance.Application.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.FirstName).NotNull().NotEmpty();
        RuleFor(c => c.LastName).NotNull().NotEmpty();
        RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotNull().NotEmpty();
        RuleFor(c => c.Group).NotNull().NotEmpty();
    }
}