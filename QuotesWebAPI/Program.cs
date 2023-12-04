/* Program.cs
 * Class for QuotesWebAPI
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Data;
using QuotesWebAPI.Extensions;
using QuotesWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication();

// Use our service extensions methods:
builder.Services.ConfigureCors();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

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

// and finally say that we are using CORS here specifying the CORS policy by name:
app.UseCors("QuotesApiCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
