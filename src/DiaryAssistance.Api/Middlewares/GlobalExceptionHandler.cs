using DiaryAssistance.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DiaryAssistance.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Extensions = { ["traceId"] = traceId }
        };

        var (statusCode, title, type) = MapException(exception);

        problemDetails.Status = statusCode;
        problemDetails.Title = title;
        problemDetails.Type = type;

        if (exception is FluentValidation.ValidationException fluentException)
        {
            var validationErrors = fluentException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            problemDetails.Extensions.Add("errors", validationErrors);
        }
        else if (HasStructuredErrors(exception, out var errors))
        {
            problemDetails.Extensions.Add("errors", errors);
        }

        if (_environment.IsDevelopment() || statusCode < 500)
        {
            problemDetails.Detail = exception.Message;
        }
        else
        {
            problemDetails.Detail = "An internal server error occurred. Please contact support.";
        }

        LogException(exception, statusCode, traceId);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int StatusCode, string Title, string Type) MapException(Exception exception)
    {
        return exception switch
        {
            FluentValidation.ValidationException =>
                (StatusCodes.Status400BadRequest,
                 "One or more validation errors occurred.",
                 "https://tools.ietf.org/html/rfc7231#section-6.5.1"),

            BadRequestException =>
                (StatusCodes.Status400BadRequest,
                 "Bad request.",
                 "https://tools.ietf.org/html/rfc7231#section-6.5.1"),

            BrokenRulesException =>
                (StatusCodes.Status400BadRequest,
                 "Business rule violation.",
                 "https://tools.ietf.org/html/rfc7231#section-6.5.1"),

            UnauthorizedException or UnauthorizedAccessException =>
                (StatusCodes.Status401Unauthorized,
                 "Unauthorized.",
                 "https://tools.ietf.org/html/rfc7235#section-3.1"),

            ForbiddenException =>
                (StatusCodes.Status403Forbidden,
                 "Forbidden.",
                 "https://tools.ietf.org/html/rfc7231#section-6.5.3"),

            NotFoundException =>
                (StatusCodes.Status404NotFound,
                 "Resource not found.",
                 "https://tools.ietf.org/html/rfc7231#section-6.5.4"),

            ConflictException =>
                (StatusCodes.Status409Conflict,
                 "Resource conflict.",
                 "https://tools.ietf.org/html/rfc7231#section-6.5.8"),

            InvalidOperationException or InternalServerException =>
                (StatusCodes.Status500InternalServerError,
                 "Internal server error.",
                 "https://tools.ietf.org/html/rfc7231#section-6.6.1"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "An unexpected error occurred.",
                 "https://tools.ietf.org/html/rfc7231#section-6.6.1")
        };
    }

    private static bool HasStructuredErrors(Exception exception, out Dictionary<string, string[]>? errors)
    {
        errors = exception switch
        {
            BadRequestException ex => ex.Errors,
            UnauthorizedException ex => ex.Errors,
            ForbiddenException ex => ex.Errors,
            ConflictException ex => ex.Errors,
            InternalServerException ex => ex.Errors,
            _ => null
        };

        return errors is { Count: > 0 };
    }

    private void LogException(Exception exception, int statusCode, string traceId)
    {
        var logLevel = statusCode >= 500 ? LogLevel.Error : LogLevel.Warning;

        _logger.Log(
            logLevel,
            exception,
            "Exception occurred. TraceId: {TraceId}, Status: {StatusCode}, Type: {ExceptionType}",
            traceId,
            statusCode,
            exception.GetType().Name
        );
    }
}
