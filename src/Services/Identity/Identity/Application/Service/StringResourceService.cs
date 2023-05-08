using Identity.Application.Abstractions;
using Identity.Setup.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Identity.Application.Service;

public class StringResourceService : IStringResourceService
{
    public JObject Resource { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StringResourceService> _logger;
    private readonly IOptions<AppSettings> _appSettings;
    
    public StringResourceService(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider, ILogger<StringResourceService> logger, IOptions<AppSettings> appSettings)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _appSettings = appSettings;
        Resource = new JObject();
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var env = serviceProvider.GetService<IHostEnvironment>();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var contentPath = env.ContentRootPath;
            var fileUrl = Path.Combine(contentPath, "lang", "en.json");
            try
            {
                using (StreamReader r = new StreamReader(fileUrl))
                {
                    var json = r.ReadToEnd();
                    Resource = JObject.Parse(json);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("---- Cannot get string resource in {0}: {1}", nameof(StringResourceService), e.Message);
                throw;
            }
        }
    }

    public JObject GetResource()
    {
        // Project not using language save in database 
        // try
        // {
        //     var env = _serviceProvider.GetService<IHostEnvironment>();
        //     var contentPath = env.ContentRootPath;
        //     var user = _httpContextAccessor.HttpContext?.User;
        //     using (StreamReader r = new StreamReader(fileUrl))
        //     {
        //         string json = r.ReadToEnd();
        //         var result = JObject.Parse(json);
        //         return result;
        //     }
        //     
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        //     throw;
        // }
        return Resource;
    }
}