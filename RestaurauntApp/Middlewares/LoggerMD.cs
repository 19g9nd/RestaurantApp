using System.Data.SqlClient;
using System.Text;
using Dapper;
using RestaurauntApp.Services.Classes;

namespace RestaurauntApp.Middlewares
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
                    request.EnableBuffering();
                    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                    await request.Body.ReadAsync(buffer, 0, buffer.Length);
                    requestContent = Encoding.UTF8.GetString(buffer);

                    request.Body.Position = 0;  // Rewind the stream to 0
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
