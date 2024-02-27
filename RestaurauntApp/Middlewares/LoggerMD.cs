using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestaurauntApp.Services.Classes;

namespace RestaurauntApp.Middlewares
{
    public class LoggerMD : IMiddleware
    {
        private readonly ILogger<LoggerMD> logger;
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public LoggerMD(ILogger<LoggerMD> logger, IConfiguration configuration, string connection)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.connectionString = connection;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (!IsLoggingEnabled())
                {
                    await next(context);
                    return;
                }

                var requestContent = await CaptureRequestBodyAsync(context.Request);

                var fullUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";

                using (var responseBodyStream = new MemoryStream())
                {
                    var originalBodyStream = context.Response.Body;
                    context.Response.Body = responseBodyStream;

                    await next(context);

                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    var responseContent = await new StreamReader(responseBodyStream).ReadToEndAsync();

                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalBodyStream);

                    var logEntry = new LogEntry
                    {
                        UserId = context.User?.Identity?.Name ?? "Guest",
                        Url = fullUrl,
                        Method = context.Request.Method,
                        StatusCode = context.Response.StatusCode,
                        RequestBody = requestContent,
                        ResponseBody = responseContent,
                        CreationDate = DateTime.UtcNow
                    };

                    await InsertLogEntryAsync(logEntry);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred in LoggerMD middleware");
                throw; 
            }
        }

        private async Task<string> CaptureRequestBodyAsync(HttpRequest request)
        {
            if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                request.Body.Position = 0;  // Rewind the stream to 0
                return Encoding.UTF8.GetString(buffer);
            }
            return null;
        }

        private bool IsLoggingEnabled()
        {
            return configuration.GetValue<bool>("Logging:EnableLogging");
        }

        private async Task InsertLogEntryAsync(LogEntry logEntry)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(@"INSERT INTO LogEntries 
                                        (UserId, Url, Method, StatusCode, RequestBody, ResponseBody, CreationDate) 
                                        VALUES 
                                        (@UserId, @Url, @Method, @StatusCode, @RequestBody, @ResponseBody, @CreationDate)",
                                                param: logEntry);
            }
        }
    }
}
