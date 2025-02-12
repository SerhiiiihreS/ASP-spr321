using ASP_spr321.Services.OTP;
using ASP_spr321.Services.Timestamp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ITimestampService, SystemTimestampService>();
//builder.Services.AddSingleton<ITimestampService, UnixTimestampService>();
builder.Services.AddTransient<ITimestampService, UnixTimestampService>();
builder.Services.AddTransient<OTPservice, OtpRand>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
