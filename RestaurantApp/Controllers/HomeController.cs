using System.Net;

public class HomeController : ControllerBase
{
    public async Task HomePageAsync(HttpListenerContext context)
    {
        try
        {
            using var writer = new StreamWriter(context.Response.OutputStream);

            var pageHtml = await File.ReadAllTextAsync("Views/Home.html");
            await writer.WriteLineAsync(pageHtml);
            await writer.FlushAsync();  // Ожидаем чтобы все данные были отправлены

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "text/html";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}

