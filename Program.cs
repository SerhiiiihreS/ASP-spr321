using ASP_SPR311.Services.Kdf;
using ASP_spr321.Data;
using ASP_spr321.Middleware;
using ASP_spr321.Models;
using ASP_spr321.Services.Kdf;
using ASP_spr321.Services.OTP;
using ASP_spr321.Services.Storage;
using ASP_spr321.Services.Timestamp;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddSingleton<ITimestampService, SystemTimestampService>();
//builder.Services.AddSingleton<ITimestampService, UnixTimestampService>();
builder.Services.AddTransient<ITimestampService, UnixTimestampService>();
builder.Services.AddTransient<OTPservice, OtpRand>();
//https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0


builder.Services.AddSingleton<IKdfService, PbKdf1Service>();
builder.Services.AddSingleton<IStorageService, FileStorageService>();
builder.Services.AddSingleton<IStorageService, BlobServiceClient>();


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

builder.Services.AddScoped<DataAccessor>();

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new DoubleBinderProvider());
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "CorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthorization(); 


app.UseSession();

app.UseAuthSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using var scope = app.Services.CreateScope();
await using var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
await dataContext.Database.MigrateAsync();

app.Run();
