using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RestaurauntApp.Services.Classes;

namespace RestaurauntApp.Middlewares 
{
    public class LoggerMD : IMiddleware
    {
        private readonly ILogger<LoggerMD> logger;
      
        private readonly IConfiguration configuration;
        private  string requestContent;
       

        public LoggerMD( ILogger<LoggerMD> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
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

            request.Body.Position = 0;  //rewinding the stream to 0
        }
        
                var logEntry = new LogEntry
                {
                    UserId = context.User.Identity.Name, // с аутентификацией
                    Url = context.Request.Path,
                    Method = context.Request.Method,
                    StatusCode = context.Response.StatusCode,
                    RequestBody = requestContent, //  логика для записи тела request
                    ResponseBody = "", //логика для записи тела response
                    Timestamp = DateTime.UtcNow
                };

                using (var connection = new SqlConnection("Server=localhost;Database=RestaurantAppDb;Integrated Security=SSPI"))
                {
                    await connection.ExecuteAsync("INSERT INTO LogEntries (UserId, Url, Method, StatusCode, RequestBody, ResponseBody, Timestamp) VALUES (@UserId, @Url, @Method, @StatusCode, @RequestBody, @ResponseBody, @Timestamp)", param: logEntry);
                }
            }
            await next.Invoke(context);
        }
    }
}