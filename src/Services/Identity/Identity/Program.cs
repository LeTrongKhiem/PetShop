using Identity.Infrastructure.Data;
using Identity.Log;
using Identity.SeedData;
using Identity.Setup;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
builder.Services.AddMvc();
builder.Services.AddAuthentication();
#region AddService
builder.Services.AddControllers();
builder.Services.AddDatabases(configuration);
builder.Services.AddAppServices(configuration);
builder.Services.AddIdentity();
builder.Services.AddCorsPolicy();
builder.Services.AddMVCRouting();
#endregion

#region Config IdentityServer
var connectionString = configuration.GetConnectionString("DefaultConnection");
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var issuerUri = configuration.GetValue<string>("Identity:IssuerUri", null);
IIdentityServerBuilder identityServerBuilder = builder.Services.AddIdentityServer(issuerUri, connectionString, migrationsAssembly);
if (builder.Environment.IsDevelopment())
{
    identityServerBuilder.AddDeveloperSigningCredential();
}
else
{
    identityServerBuilder.AddSigningCredential("CN=PetShopAuth");
}
#endregion

var app = builder.Build();
Log.Logger = ConfigLogger.GetLoggerConfiguration().CreateLogger();
SeedData.EnsureSeedData(app.Services);
app.UseAuthentication();
app.UseIdentityServer();
app.ConfigureCors();
app.UseStaticFiles();
app.ConfigureRouting();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserContext>();
    db.Database.Migrate();
}
app.Run();