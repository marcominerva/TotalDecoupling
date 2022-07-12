namespace TotalDecoupling.BusinessLayer.Models;

public class OperationResult<T> : IOperationResult
{
    public bool Success { get; }

    public T Content { get; }

    public FailureReason FailureReason { get; }

    public Exception Error { get; }

    private readonly string errorMessage;
    public string ErrorMessage => errorMessage ?? Error?.Message;

    private readonly string errorDetail;
    public string ErrorDetail => errorDetail ?? Error?.InnerException?.Message;

    public IEnumerable<ValidationError> ValidationErrors { get; }

    internal OperationResult(bool success = true, T content = default, FailureReason failureReason = FailureReason.None, string message = null, string detail = null, Exception error = null, IEnumerable<ValidationError> validationErrors = null)
    {
        Success = success;
        Content = content;
        FailureReason = failureReason;
        errorMessage = message;
        errorDetail = detail;
        Error = error;
        ValidationErrors = validationErrors;
    }

    internal static OperationResult<T> Ok(T content = default)
        => new(success: true, content);

    internal static OperationResult<T> Fail(FailureReason failureReason, ValidationError validationError)
        => new(false, failureReason: failureReason, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult<T> Fail(FailureReason failureReason, string message, ValidationError validationError)
        => new(false, failureReason: failureReason, message: message, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult<T> Fail(FailureReason failureReason, string message, string detail, ValidationError validationError)
        => new(false, failureReason: failureReason, message: message, detail: detail, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult<T> Fail(FailureReason failureReason, T content, ValidationError validationError)
        => new(false, failureReason: failureReason, content: content, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult<T> Fail(FailureReason failureReason, Exception error, ValidationError validationError)
        => new(false, failureReason: failureReason, error: error, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult<T> Fail(FailureReason failureReason, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, validationErrors: validationErrors);

    internal static OperationResult<T> Fail(FailureReason failureReason, string message, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, message: message, validationErrors: validationErrors);

    internal static OperationResult<T> Fail(FailureReason failureReason, string message, string detail, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, message: message, detail: detail, validationErrors: validationErrors);

    internal static OperationResult<T> Fail(FailureReason failureReason, T content, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, content: content, validationErrors: validationErrors);

    internal static OperationResult<T> Fail(FailureReason failureReason, Exception error, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, error: error, validationErrors: validationErrors);

    public static implicit operator OperationResult<T>(T value)
        => Ok(value);

    public static implicit operator OperationResult<T>(OperationResult result)
        => new(result.Success, default, result.FailureReason, result.ErrorMessage, result.ErrorDetail, result.Error, result.ValidationErrors);
}
