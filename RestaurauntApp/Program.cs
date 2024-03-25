using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Data;
using RestaurauntApp.Middlewares;
using RestaurauntApp.Repositories;
using RestaurauntApp.Repositories.Base;
using RestaurauntApp.Services;
using RestaurauntApp.Services.Base;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MenuDb");
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddDbContext<RestaurantAppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password = new PasswordOptions
    {
        RequireDigit = false,
        RequiredLength = 4,
        RequireNonAlphanumeric = false,
        RequireUppercase = false,
        RequireLowercase = false
    };
})
    .AddEntityFrameworkStores<RestaurantAppDbContext>();


builder.Services.AddTransient(provider =>
{
    return new LoggerMD(provider.GetRequiredService<ILogger<LoggerMD>>(), provider.GetRequiredService<IConfiguration>(), connectionString);
});

builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IMenuRepository, MenuRepository>(provider =>
{
#pragma warning disable CS8604 // Possible null reference argument.
    return new MenuRepository(connectionString);
#pragma warning restore CS8604 // Possible null reference argument.
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();