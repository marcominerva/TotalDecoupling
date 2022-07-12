namespace TotalDecoupling.BusinessLayer.Models;

public class OperationResult : IOperationResult
{
    public bool Success { get; }

    public FailureReason FailureReason { get; }

    public Exception Error { get; }

    private readonly string errorMessage;
    public string ErrorMessage => errorMessage ?? Error?.Message;

    private readonly string errorDetail;
    public string ErrorDetail => errorDetail ?? Error?.InnerException?.Message;

    public IEnumerable<ValidationError> ValidationErrors { get; }

    internal OperationResult(bool success = true, FailureReason failureReason = FailureReason.None, string message = null, string detail = null, Exception error = null, IEnumerable<ValidationError> validationErrors = null)
    {
        Success = success;
        FailureReason = failureReason;
        errorMessage = message;
        errorDetail = detail;
        Error = error;
        ValidationErrors = validationErrors;
    }

    internal static OperationResult Ok()
        => new(success: true);

    internal static OperationResult Fail(FailureReason failureReason, ValidationError validationError)
        => new(false, failureReason: failureReason, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult Fail(FailureReason failureReason, string message, ValidationError validationError)
        => new(false, failureReason: failureReason, message: message, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult Fail(FailureReason failureReason, string message, string detail, ValidationError validationError)
        => new(false, failureReason: failureReason, message: message, detail: detail, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult Fail(FailureReason failureReason, Exception error, ValidationError validationError)
        => new(false, failureReason: failureReason, error: error, validationErrors: new ValidationError[1] { validationError });

    internal static OperationResult Fail(FailureReason failureReason, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, validationErrors: validationErrors);

    internal static OperationResult Fail(FailureReason failureReason, string message, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, message: message, validationErrors: validationErrors);

    internal static OperationResult Fail(FailureReason failureReason, string message, string detail, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, message: message, detail: detail, validationErrors: validationErrors);

    internal static OperationResult Fail(FailureReason failureReason, Exception error, IEnumerable<ValidationError> validationErrors = null)
        => new(false, failureReason: failureReason, error: error, validationErrors: validationErrors);
}
