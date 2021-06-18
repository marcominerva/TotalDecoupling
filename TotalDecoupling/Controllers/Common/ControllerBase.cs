using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using TotalDecoupling.BusinessLayer.Models;

namespace TotalDecoupling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected IActionResult CreateResponse(OperationResult operationResult, int? responseStatusCode = null)
        {
            if (operationResult.Success)
            {
                return StatusCode(responseStatusCode.GetValueOrDefault(StatusCodes.Status204NoContent));
            }

            return Problem(FailureReasonToStatusCode(operationResult.FailureReason), null, operationResult.ErrorMessage, operationResult.ErrorDetail, operationResult.ValidationErrors);
        }

        protected IActionResult CreateResponse<T>(OperationResult<T> operationResult, int? responseStatusCode = null)
            => CreateResponse(operationResult, null, null, responseStatusCode);

        protected IActionResult CreateResponse<T>(OperationResult<T> operationResult, string actionName, object routeValues = null, int? responseStatusCode = null)
        {
            if (operationResult.Success)
            {
                if (operationResult.Content != null)
                {
                    if (!string.IsNullOrWhiteSpace(actionName))
                    {
                        var routeValueDictionary = new RouteValueDictionary(routeValues);
                        return CreatedAtRoute(actionName, routeValueDictionary, operationResult.Content);
                    }
                    else if (operationResult.Content is ByteArrayFileContent fileContent)
                    {
                        return File(fileContent.Content, fileContent.ContentType, fileContent.DownloadFileName);
                    }

                    return Ok(operationResult.Content);
                }

                return StatusCode(responseStatusCode.GetValueOrDefault(StatusCodes.Status204NoContent));
            }

            return Problem(FailureReasonToStatusCode(operationResult.FailureReason), operationResult.Content, operationResult.ErrorMessage, operationResult.ErrorDetail, operationResult.ValidationErrors);
        }

        protected IActionResult Problem(IEnumerable<ValidationError> validationErrors)
            => Problem(StatusCodes.Status400BadRequest, content: null, title: null, detail: null, validationErrors: validationErrors);

        protected IActionResult Problem(int statusCode, object content, IEnumerable<ValidationError> validationErrors = null)
            => Problem(statusCode, content, title: null, detail: null, validationErrors: validationErrors);

        protected IActionResult Problem(HttpStatusCode statusCode, object content, IEnumerable<ValidationError> validationErrors = null)
            => Problem((int)statusCode, content, title: null, detail: null, validationErrors: validationErrors);

        protected IActionResult Problem(int statusCode, string title = null, string detail = null, IEnumerable<ValidationError> validationErrors = null)
            => Problem(statusCode, content: null, title, detail, validationErrors: validationErrors);

        protected IActionResult Problem(HttpStatusCode statusCode, string title = null, string detail = null, IEnumerable<ValidationError> validationErrors = null)
            => Problem((int)statusCode, content: null, title, detail, validationErrors: validationErrors);

        protected IActionResult Problem(Exception error, int statusCode = StatusCodes.Status500InternalServerError, IEnumerable<ValidationError> validationErrors = null)
            => Problem(statusCode, content: null, error.Message, error.InnerException?.Message, validationErrors: validationErrors);

        protected IActionResult Problem(Exception error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, IEnumerable<ValidationError> validationErrors = null)
            => Problem((int)statusCode, content: null, error.Message, error.InnerException?.Message, validationErrors: validationErrors);

        private IActionResult Problem(int statusCode, object content = null, string title = null, string detail = null, IEnumerable<ValidationError> validationErrors = null)
        {
            if (content != null)
            {
                return StatusCode(statusCode, content);
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = title ?? ReasonPhrases.GetReasonPhrase(statusCode),
                Detail = detail,
                Instance = HttpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            if (validationErrors?.Any() ?? false)
            {
                problemDetails.Extensions.Add("errors", validationErrors);
            }

            return StatusCode(statusCode, problemDetails);
        }

        private static int FailureReasonToStatusCode(FailureReason failureReason, int? defaultResponseStatusCode = null)
            => failureReason switch
            {
                FailureReason.ItemNotFound => StatusCodes.Status404NotFound,
                FailureReason.Forbidden => StatusCodes.Status403Forbidden,
                FailureReason.ClientError => StatusCodes.Status400BadRequest,
                FailureReason.DatabaseError => StatusCodes.Status500InternalServerError,
                FailureReason.GenericError => StatusCodes.Status500InternalServerError,
                _ => defaultResponseStatusCode.GetValueOrDefault(StatusCodes.Status500InternalServerError)
            };
    }
}
