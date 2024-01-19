using RestaurauntApp.Middlewares;
using RestaurauntApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<LoggerMD>();
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IMenuRepository, MenuRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MenuDb");
    return new MenuRepository(connectionString);
});


//     builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//         .AddCookie(options =>
//         {
//             options.LoginPath = "/Auth/SinIn";
//             options.ReturnUrlParameter = "returnUrl";
//         });
//     builder.Services.AddAuthorization();



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
