using System.Collections.ObjectModel;

namespace AuthenticationProvider.Application.Errors;
public class AppError
{
    public string Code {get;}
    public string Title {get;}
    public string Message {get;}

    public AppError(string code, string title, string message)
    {
        Code = code;
        Title = title;
        Message = message;
    }

    public static NotFoundError NotFound(string subject, string target, string value)
    {
        return new NotFoundError($"{subject.ToUpperInvariant()}.{target.ToUpperInvariant()}", 
        $"{subject} was not found.",
        $"{subject} with the {target} equal to {value} was not found.");
    }

    public static InvalidInputError Validation(string subject, List<ValidationErrorEntry> validationErrorEntries)
    {
        return new ValidationError($"{subject.ToUpperInvariant()}", 
        $"Failed to validate {subject}.",
        $"One or more fields on {subject} were invalid. See validationErrorEntries for more details.",
        validationErrorEntries);
    }

    public static AppError InternalError(string title, string message)
    {
        return new AppError("INTERNAL", title, message);
    }
}
public class NotFoundError : AppError
{
    public NotFoundError(string code, string title, string message)
    : base("NOT_FOUND." + code, title, message)
    {
    }
}

public class UnauthorizedError : AppError
{
    public  UnauthorizedError(string code, string title, string message)
    : base("UNAUTHORIZED." + code, title, message)
    {
    }

    public static UnauthorizedError InvalidUsernameOrPassword()
    {
        return new UnauthorizedError("USERNAME_OR_PASSWORD", "Invalid username or password.", 
            "User with the coresponding username was not found or the provided password did not match.");
    }
}


public class InvalidInputError : AppError
{
    public InvalidInputError(string code, string title, string message)
    : base("INVALID_INPUT." + code, title, message)
    {
    }
}

public class ValidationError : InvalidInputError
{
    public ReadOnlyCollection<ValidationErrorEntry> ValidationErrorEntries => _validationErrorEntries.AsReadOnly();
    private List<ValidationErrorEntry> _validationErrorEntries;


    public ValidationError(string code, string title, string message, List<ValidationErrorEntry> validationErrorEntriess)
    : base("INVALID_INPUT.VALIDATION." + code, title, message)
    {
        _validationErrorEntries = validationErrorEntriess;
    }
    
}
public class ValidationErrorEntry
{
    public string Field {get;}
    public string Message {get;}

    public ValidationErrorEntry(string field, string message)
    {
        Field = field;
        Message = message;
    }

    public static ValidationErrorEntry AlreadyExistWith(string subject, string field)
    {
        return new ValidationErrorEntry(field, $"{subject} with the same {field} already exists");
    }
        public static ValidationErrorEntry PasswordAreNotTheSame(string passwordField, string verifyPasswordField)
    {
        return new ValidationErrorEntry(verifyPasswordField, $"{passwordField} and {verifyPasswordField} are not the same");
    }
}