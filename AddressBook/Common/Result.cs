namespace AddressBook.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public object? Data { get; }

    protected Result(bool isSuccess, string message, object? data = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }

    public static Result Success(string message = "Operation successful", object? data = null)
    {
        return new Result(true, message, data);
    }

    public static Result Failure(string message, object? data = null)
    {
        return new Result(false, message, data);
    }
}

public class Result<T> : Result
{
    public new T? Data => (T?)base.Data;

    protected Result(bool isSuccess, string message, T? data = default)
        : base(isSuccess, message, data)
    {
    }

    public static Result<T> Success(string message = "Operation successful", T? data = default)
    {
        return new Result<T>(true, message, data);
    }

    public static Result<T> Failure(string message, T? data = default)
    {
        return new Result<T>(false, message, data);
    }
}