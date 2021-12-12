using System;
using System.Collections.Generic;

namespace TotalDecoupling.BusinessLayer.Models;

public interface IOperationResult
{
    Exception Error { get; }

    string ErrorDetail { get; }

    string ErrorMessage { get; }

    FailureReason FailureReason { get; }

    bool Success { get; }
    IEnumerable<ValidationError> ValidationErrors { get; }
}
