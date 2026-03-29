namespace BookStork.Application.Abstractions;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Error { get; }
 
    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = string.Empty;
    }
 
    private Result(string error)
    {
        IsSuccess = false;
        Error = error;
    }
 
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}
 
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
 
    private Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
 
    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
}
