using AuthenticationProvider.Application.Dtos.AppUserDtos;
using FluentValidation;

namespace AuthenticationProvider.Application.Validators;

public class RegisterAppUserFormValidator : AbstractValidator<RegisterAppUserForm>
{

    public RegisterAppUserFormValidator()
    {
        RuleFor(f=>f.Username).ValidateUsername();
        RuleFor(f=>f.Email).CustomNotEmpty().EmailAddress();
        RuleFor(f=>f.Password).ValidatePassword();
        RuleFor(f=>f.VerifyPassword).Must((form,confirmPassword)=>form.Password == confirmPassword).WithMessage("Passwords must match");
    }
}