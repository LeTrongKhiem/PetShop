using Newtonsoft.Json.Linq;

namespace Identity.Application.Abstractions;

public interface IStringResourceService
{
    JObject Resource { get; set; }
    JObject GetResource();
}
