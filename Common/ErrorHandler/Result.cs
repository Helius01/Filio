using Filio.Common.ErrorHandler.Abstractions;

namespace Filio.Common.ErrorHandler;

/// <summary>
/// The result used to function results
/// </summary>
public readonly struct Result<R, E> where E : BaseRecoverableError
{
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public R? Value { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public E? Error { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public bool IsSuccess { get; }

    private Result(R value, E error, bool isSuccess)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
    }
    public static Result<R, E> Success(R value) => new(value, default, true);
    public static Result<R, E> Failure(E error) => new(default, error, false);
}
