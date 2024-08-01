using System.Text;

namespace Patika_Hafta1_Odev.Handler
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request
            context.Request.EnableBuffering();
            var requestBody = await ReadStreamInChunks(context.Request.Body);
            _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path} {requestBody}");
            context.Request.Body.Position = 0;

            // Log the response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Outgoing Response: {context.Response.StatusCode} {responseBodyText}");

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> ReadStreamInChunks(Stream stream)
        {
            const int bufferLength = 4096;
            var buffer = new byte[bufferLength];
            var stringBuilder = new StringBuilder();
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, bufferLength)) > 0)
            {
                stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            return stringBuilder.ToString();
        }
    }
}
