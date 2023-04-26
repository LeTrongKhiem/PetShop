using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
IConfiguration configuration = builder.Configuration;
IHostEnvironment environment = builder.Environment;

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
    .SetIsOriginAllowed((host) => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.File(new CompactJsonFormatter(), $"logs/log.txt", rollOnFileSizeLimit: true,
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Logger.Information("Gateway Starting...");

builder.Services.AddOcelot(configuration);

if (environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseHealthChecksUI(setup =>
{
    setup.UIPath = "/api-health";
    setup.ApiPath = "/api-health-json";
});

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.UseOcelot().Wait();