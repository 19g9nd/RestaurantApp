using RestaurauntApp.Middlewares;
using RestaurauntApp.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MenuDb");

builder.Services.AddTransient(provider =>
{
    return new LoggerMD(provider.GetRequiredService<ILogger<LoggerMD>>(), provider.GetRequiredService<IConfiguration>(), connectionString);
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMenuRepository, MenuRepository>(provider =>
{
    return new MenuRepository(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<LoggerMD>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();