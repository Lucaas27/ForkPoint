// Ignore Spelling: Middleware


using System.Diagnostics;

namespace ForkPoint.API.Middlewares;

public class ElapsedTimeMiddleware(ILogger<ElapsedTimeMiddleware> logger) : IMiddleware
{
    private const int ThresholdInSeconds = 4;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        await next.Invoke(context);

        stopwatch.Stop();

        var elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000;
        var method = context.Request.Method;
        var path = context.Request.Path;

        if (elapsedSeconds > ThresholdInSeconds)
            logger.LogWarning("Request [{Method}] {Path} took {ElapsedMilliseconds} seconds", method, path,
                elapsedSeconds);
    }
}