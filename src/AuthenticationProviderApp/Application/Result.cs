using System.Collections.ObjectModel;
using AuthenticationProvider.Application.Errors;

namespace AuthenticationProvider.Application;

public class Result
{
    public bool IsSuccess {get; private set;}
    public bool IsError => !IsSuccess;
    public AppError Error 
    {
        get 
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the error, result is successful");
            }
            
            return field ?? throw new InvalidOperationException("A failed result must contain an error.");    
        }
    }


    protected Result(bool isSuccess = true)
    {
        IsSuccess = isSuccess;
    }

    protected Result(AppError error)
    {
        IsSuccess = false;
        Error = error;
    }

    public void Switch(Action onSuccess, Action<AppError> onFailure)
    {
        if(IsSuccess)
            onSuccess();
        else
            onFailure(Error);
    }

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Failure<TValue>(AppError error) => new(error);
}

public sealed class Result<TValue> : Result  
{
    public TValue Value
    {  
        get 
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the value, result is a failure.");
            }
            
            return field ?? throw new InvalidOperationException("A successful result must contain a value.");    
        }
    }
    
    internal Result(TValue value) : base()
    {
        Value = value;
    }

    public Result(AppError error) : base(error){}

    public void Switch(Action<TValue> onSuccess, Action<AppError> onFailure)
    {
        if(IsSuccess)
            onSuccess(Value);
        else
            onFailure(Error);
    }

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<AppError, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value) : onFailure(Error);
    }

    public Result<TValue> IfPresent(Action<TValue> action) {
        if(IsSuccess) action(Value);
        return this;
    }
}