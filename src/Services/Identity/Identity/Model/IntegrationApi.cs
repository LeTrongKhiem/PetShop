namespace Identity.Model;

public class IntegrationApi
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public bool Active { get; set; }
    public string Url { get; set; }
    public Swagger Swagger { get; set; }
}
public class Swagger
{
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string Url { get; set; }
}