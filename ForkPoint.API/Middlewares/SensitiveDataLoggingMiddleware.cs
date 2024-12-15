using System.Text.Json;

namespace ForkPoint.API.Middlewares;

public class SensitiveDataLoggingMiddleware(IConfiguration config) : IMiddleware
{
    private readonly string _maskingKey = config["Logging:SensitiveData:Mask"] ?? "**MASKED**";

    private readonly string[] _sensitiveFields =
        config.GetSection("Logging:SensitiveData:Keywords").Get<string[]>() ?? [];

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
                    newJson[element.Name] = _maskingKey;
                else
                    newJson[element.Name] = value;
            }

            return JsonSerializer.Serialize(newJson);
        }
        catch
        {
            return json;
        }
    }
}