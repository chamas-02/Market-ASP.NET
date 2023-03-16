using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<MarketGOContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMarketGO"));
});

builder.Services.AddNotyf(Configuration => { Configuration.DurationInSeconds = 3; Configuration.IsDismissable = true; Configuration.Position = NotyfPosition.TopRight; });

// Giải mã tiếng việt
builder.Services.AddSingleton<HtmlEncoder>(
           HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
