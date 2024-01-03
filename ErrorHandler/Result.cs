using System.Globalization;
using Filio.ErrorHandler.Abstraction;

namespace Filio.ErrorHandler;

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
    public static Result<R, E> Success(R value) => new Result<R, E>(value, default, true);
    public static Result<R, E> Failure(E error) => new Result<R, E>(default, error, false);
}
