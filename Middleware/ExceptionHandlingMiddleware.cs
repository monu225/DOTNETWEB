using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace WEBAPI_CRUD.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error occurred.");
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, "A database error occurred.");
        }
        catch (FormatException ex)
        {
            _logger.LogWarning(ex, "Invalid formatted input was processed.");
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, "Invalid request format.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error occurred.");
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
