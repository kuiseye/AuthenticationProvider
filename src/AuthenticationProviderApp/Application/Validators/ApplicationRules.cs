using FluentValidation;

namespace AuthenticationProvider.Application.Validators;

public static class ApplicationRules
{
    public static IRuleBuilderOptions<T, string> CustomNotEmpty<T>(this IRuleBuilder<T, string> rule) 
         => rule.NotEmpty().WithMessage("{PropertyName} must not be empty");
    public static IRuleBuilderOptions<T, string> CustomMimLength<T>(this IRuleBuilder<T, string> rule, int minLength) 
         => rule.MinimumLength(minLength).WithMessage($"{{PropertyName}} must be at least {minLength} characters long");

    public static IRuleBuilderOptions<T, string> CustomMaxLength<T>(this IRuleBuilder<T, string> rule, int maxLength) 
         => rule.MaximumLength(maxLength).WithMessage($"{{PropertyName}} must be at most {maxLength} characters long");
}