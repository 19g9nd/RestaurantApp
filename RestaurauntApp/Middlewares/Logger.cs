namespace RestaurauntApp.Middlewares
{
    public class Logger : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}