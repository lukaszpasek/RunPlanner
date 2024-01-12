using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RunPlanner.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the HTTP request pipeline.
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddExceptionHandler(options =>
    {
        options.ExceptionHandlingPath = "/Home/Error";
    });

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    builder.Services.AddHsts(options =>
    {
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(30);
    });

    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect;
        options.HttpsPort = 443;
    });
}

// Rest of the code remains the same


builder.Services.AddControllersWithViews();

builder.Services.AddRouting();

builder.Services.AddAuthorization();

builder.Services.AddDbContext<GpxDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ...

// Ten fragment kodu musi być przed Build, ponieważ wywołuje Migrate w klasie GpxDbContext
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GpxDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();
