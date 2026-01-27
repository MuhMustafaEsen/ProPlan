using Microsoft.IO;
using NLog;
using System.Diagnostics;
using System.Text;

namespace ProPlan.WebApi.Middleware
{
    public class RequestResponseLoggingMiddleware

    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _streamManager = new();
        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Request
            var requestBody = await ReadRequestBody(context.Request);

            // Response
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _streamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            stopwatch.Stop();
            var responseText = await ReadResponseBody(context.Response);

                _logger.Info(@"
                    HTTP LOG
                    --------------------------------
                    Method     : {method}
                    Path       : {path}
                    Query      : {query}
                    Request    : {request}
                    Response   : {response}
                    StatusCode : {statusCode}
                    Duration   : {duration} ms
                    --------------------------------",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            requestBody,
            responseText,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);

            await responseBody.CopyToAsync(originalBodyStream);
        }
        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            using var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return body;
        }
        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}
