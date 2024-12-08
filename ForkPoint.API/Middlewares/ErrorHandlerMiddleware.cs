﻿// Ignore Spelling: Middleware

using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Domain.Exceptions;
using System.Net.Mime;
using System.Text.Json;

namespace ForkPoint.API.Middlewares;

/// <summary>
/// Middleware to handle exceptions and provide a consistent error response format.
/// </summary>
/// <param name="logger">The logger instance to log errors.</param>
public class ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger) : IMiddleware
{
    /// <summary>
    /// Invokes the next middleware in the pipeline and handles any exceptions that occur.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="next">The next middleware to invoke.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when a resource is not found.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFoundEx)
        {
            await HandleExceptionAsync(context, StatusCodes.Status404NotFound, notFoundEx.Message);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Something went wrong. Please try again.");
        }
    }

    /// <summary>
    /// Handles exceptions by logging the error and writing a JSON response with the error details.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <param name="message">The error message to return in the response.</param>
    private async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        logger.LogError("An error occurred: {Message}", message);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var errorDetails = new CustomException(false, statusCode, message);

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }
}
