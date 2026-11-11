using AuthenticationProvider.Application.Dtos.AppUserDtos;
using FluentValidation;

namespace AuthenticationProvider.Application.Validators;

public class LoginAppUserFormValidator : AbstractValidator<LoginAppUserForm>
{

    public LoginAppUserFormValidator()
    {
        RuleFor(f=>f.Username).ValidateUsername();
        RuleFor(f=>f.Password).ValidatePassword();
    }
}