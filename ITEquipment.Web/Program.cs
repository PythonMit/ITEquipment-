using Hangfire;
using Hangfire.SqlServer;
using ITEquipment.Interfaces;
using ITEquipment.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using ITEquipment.Web.Middleware.ErrorHandlingMiddlewares;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Security setting
    options.Cookie.IsEssential = true; // Ensure session is always stored
});
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IAccessories, AccessoriesManageService>();
builder.Services.AddScoped<IAccount, AuthManageService>();
builder.Services.AddScoped<IStatus, StatusManageService>();
builder.Services.AddScoped<IEquipment, EquipmentManageService>();
builder.Services.AddScoped<ICommonMethods, CommonMethodsService>();
builder.Services.AddScoped<IManufacturer, ManufacturerManageSrvice>();
builder.Services.AddScoped<ILocation, LocationManageService>();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.Zero,
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          }));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<JobScheduler>();

var app = builder.Build();
app.UseHangfireDashboard();
using (var scope = app.Services.CreateScope())
{
    var jobScheduler = scope.ServiceProvider.GetRequiredService<JobScheduler>();
    jobScheduler.ScheduleDailyEmailJob();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.UseHangfireDashboard();
//var serviceProvider = app.Services;
//var bookingService = serviceProvider.GetRequiredService<JobScheduler>();
//RecurringJob.AddOrUpdate("daily-email-job", () => bookingService.SendBookingEndEmailReminders(), Cron.Daily);
app.Run();
