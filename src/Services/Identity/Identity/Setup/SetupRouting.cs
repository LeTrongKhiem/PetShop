using System.Text.Json.Serialization;

namespace Identity.Setup
{
    public static class SetupRouting
    {
        public static IServiceCollection AddMVCRouting(this IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest)
                .AddJsonOptions(options =>
                {
                    //options.SerializerSettings.Formatting = Formatting.Indented;
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddRazorPagesOptions(options =>
                {
                    //options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            return services;
        }

        public static IApplicationBuilder ConfigureRouting(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            return app;
        }
    }
}
