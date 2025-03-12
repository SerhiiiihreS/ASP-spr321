using ASP_SPR311.Middleware;
using ASP_SPR311.Services.Kdf;
using ASP_spr321.Data;
using ASP_spr321.Services.Kdf;
using ASP_spr321.Services.OTP;
using ASP_spr321.Services.Storage;
using ASP_spr321.Services.Timestamp;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IKdfService, PbKdf1Service>();
builder.Services.AddSingleton<IStorageService, FileStorageService>();

builder.Services.AddSingleton<ITimestampService, SystemTimestampService>();
//builder.Services.AddSingleton<ITimestampService, UnixTimestampService>();
builder.Services.AddTransient<ITimestampService, UnixTimestampService>();
builder.Services.AddTransient<OTPservice, OtpRand>();
//https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDbContext<DataContext>(
    options =>
    options
    .UseSqlServer(
        builder.Configuration
        .GetConnectionString("LocalMs"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); 

app.UseSession();

app.UseAuthSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
