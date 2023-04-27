using Identity.Setup;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

{
    builder.Services.AddAppServices(configuration);
}

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();