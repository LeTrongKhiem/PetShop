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
    

    public JObject GetResource()
    {
        throw new NotImplementedException();
    }
}