using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Entities;
using Identity.Setup;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

#region AddService
builder.Services.AddControllers();
builder.Services.AddDatabases(configuration);
builder.Services.AddAppServices(configuration);
builder.Services.AddIdentity();
#endregion
var issuerUri = configuration.GetValue<string>("Identity:IssuerUri", null!);
var builderService = builder.Services
    .AddIdentityServer(opts =>
    {
        opts.Events.RaiseErrorEvents = true;
        opts.Events.RaiseInformationEvents = true;
        opts.Events.RaiseFailureEvents = true;
        opts.Events.RaiseSuccessEvents = true;

        if (!string.IsNullOrWhiteSpace(issuerUri))
        {
            opts.IssuerUri = issuerUri;
        }
    })
    .AddAspNetIdentity<ApplicationUser>();
var app = builder.Build();

app.UseIdentityServer();
app.MapGet("/", () => "Hello World!");

app.Run();