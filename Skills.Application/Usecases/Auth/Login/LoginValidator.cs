using FluentValidation;

namespace Skills.Application.Usecases.Auth.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(l => l.Username)
            .NotEmpty();

        RuleFor(l => l.Password)
            .NotEmpty();
    }
}