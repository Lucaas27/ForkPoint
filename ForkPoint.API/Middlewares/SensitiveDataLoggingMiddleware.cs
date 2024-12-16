using System.Text.Json;

namespace ForkPoint.API.Middlewares;

/// <summary>
///     Middleware to mask sensitive data in the request body before logging.
/// </summary>
/// <param name="config">The configuration object to retrieve masking settings.</param>
public class SensitiveDataLoggingMiddleware(IConfiguration config) : IMiddleware
{
    /// <summary>
    ///     The key used to mask sensitive data.
    /// </summary>
    private readonly string _maskingKey = config["Logging:SensitiveData:Mask"] ?? "**MASKED**";

    /// <summary>
    ///     The list of sensitive fields to be masked.
    /// </summary>
    private readonly string[] _sensitiveFields =
        config.GetSection("Logging:SensitiveData:Keywords").Get<string[]>() ?? [];


    /// <summary>
    ///     Middleware invocation method to process the HTTP request.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var originalBodyStream = context.Request.Body;
        using var requestReader = new StreamReader(originalBodyStream);
        var requestBody = await requestReader.ReadToEndAsync();

        // Check if the request body is not empty
        if (!string.IsNullOrEmpty(requestBody))
        {
            // Mask sensitive data in the request body
            var maskedBody = MaskSensitiveData(requestBody);

            // Create a new memory stream to hold the masked body
            var requestStream = new MemoryStream();
            var streamWriter = new StreamWriter(requestStream);

            // Write the masked body to the memory stream
            await streamWriter.WriteAsync(maskedBody);
            await streamWriter.FlushAsync();

            // Reset the stream position to the beginning
            requestStream.Position = 0;

            // Replace the original request body with the masked version
            context.Request.Body = requestStream;
        }

        await next(context);
    }


    /// <summary>
    ///     Masks sensitive data in the provided JSON string.
    /// </summary>
    /// <param name="json">The JSON string to be processed.</param>
    /// <returns>The JSON string with sensitive data masked.</returns>
    private string MaskSensitiveData(string json)
    {
        try
        {
            var jsonDocument = JsonDocument.Parse(json);
            var newJson = new Dictionary<string, object>();

            foreach (var element in jsonDocument.RootElement.EnumerateObject())
            {
                var value = element.Value.ToString();
                if (_sensitiveFields.Any(field => element.Name.Contains(field, StringComparison.OrdinalIgnoreCase)))
                {
                    newJson[element.Name] = _maskingKey;
                } else
                {
                    newJson[element.Name] = value;
                }
            }

            return JsonSerializer.Serialize(newJson);
        }
        catch
        {
            return json;
        }
    }
}