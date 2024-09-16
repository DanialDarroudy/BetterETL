using BetterETLProject;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
        serviceName: "BetterETLProject", serviceVersion: "1.0.0"));

    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.ParseStateValues = true;
    options.AddConsoleExporter();
});

builder.Services.AddControllersWithViews();
DependencyInjection.InjectDependencies(builder.Services);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();