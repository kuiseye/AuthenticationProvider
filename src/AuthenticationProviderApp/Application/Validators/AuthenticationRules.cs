using FluentValidation;

namespace AuthenticationProvider.Application.Validators;

public static class AuthenticationRules
{
    const int MinUsernameLength = 3;
    const int MaxUsernameLength = 256;

    public static IRuleBuilderOptions<T, string> ValidateUsername<T>(this IRuleBuilder<T,string> rule ) 
         => rule.NotEmpty()
                .CustomMimLength(MinUsernameLength)
                .CustomMaxLength(MaxUsernameLength);


    const int MinPasswordLength = 8;
    const int MaxPassowrdLength = 512;

        public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilder<T,string> rule ) 
         => rule.CustomNotEmpty()
                .CustomMimLength(MinPasswordLength)
                .CustomMaxLength(MaxPassowrdLength)
                .Matches("[A-Z]").WithMessage("{PropertyName} must contain at least one capital letter")
                .Matches("[0-9]").WithMessage("{PropertyName} must contain at least one number");
}