using System.Data.SqlClient;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestaurauntApp.Infrastructure.Services.Classes;

namespace RestaurauntApp.Infrastructure.Middlewares
{
    public class LoggerMD : IMiddleware
    {
        private readonly ILogger<LoggerMD> logger;
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private string requestContent;

        public LoggerMD(ILogger<LoggerMD> logger, IConfiguration configuration, string connection)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.connectionString = connection;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isEnabled = configuration.GetValue<bool>("Logging:EnableLogging");

            if (isEnabled)
            {
                var request = context.Request;
                if (request.Method == HttpMethods.Post && request.ContentLength > 0)
                {
                    // Enable buffering by replacing the original stream with a buffered stream
                    var originalRequestBody = request.Body;
                    try
                    {
                        using (var memStream = new MemoryStream())
                        {
                            request.Body = memStream;

                            // Copy the request body to the memory stream
                            await originalRequestBody.CopyToAsync(memStream);
                            memStream.Seek(0, SeekOrigin.Begin); // Rewind the stream
                            requestContent = await new StreamReader(memStream).ReadToEndAsync();
                        }
                    }
                    finally
                    {
                        // Restore the original request body
                        request.Body = originalRequestBody;
                    }
                }

                var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}";

                // Capture the response content
                var originalBodyStream = context.Response.Body;
                using (var responseBodyStream = new MemoryStream())
                {
                    context.Response.Body = responseBodyStream;
                    await next(context);
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    var responseContent = await new StreamReader(responseBodyStream).ReadToEndAsync();

                    var logEntry = new LogEntry
                    {
                        UserId = context.User.Identity.Name is null ? "Guest" : context.User.Identity.Name.ToString(),
                        Url = fullUrl,
                        Method = context.Request.Method,
                        StatusCode = context.Response.StatusCode,
                        RequestBody = requestContent,
                        ResponseBody = responseContent,
                        CreationDate = DateTime.UtcNow
                    };

                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.ExecuteAsync("INSERT INTO LogEntries (UserId, Url, Method, StatusCode, RequestBody, ResponseBody, CreationDate) VALUES (@UserId, @Url, @Method, @StatusCode, @RequestBody, @ResponseBody, @CreationDate)", param: logEntry);
                    }

                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalBodyStream);
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}

