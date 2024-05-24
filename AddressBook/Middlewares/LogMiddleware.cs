namespace AddressBook.Middlewares;

public class LogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogMiddleware> _logger;

    public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestLog = await FormatRequest(context.Request);
        _logger.LogInformation(requestLog);

        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            // Call the next middleware in the pipeline
            await _next(context);

            var responseLog = await FormatResponse(context.Response);
            _logger.LogInformation(responseLog);

            // Copy the response body to the original stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();

        // Read request body
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);

        var logMessage = $"Request: {request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString}\n";

        foreach (var (key, value) in request.Headers)
        {
            logMessage += $"{key}: {value}\n";
        }

        logMessage += $"Body: {body}";

        return logMessage.ToString();
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        var logMessage = $"Response: {response.StatusCode}\n";

        foreach (var (key, value) in response.Headers)
        {
            logMessage += $"{key}: {value}\n";
        }

        logMessage += $"Body: {body}";

        return logMessage.ToString();
    }
}