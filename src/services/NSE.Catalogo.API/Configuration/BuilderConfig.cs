using NSE.Catalogo.API;

namespace NSE.Identidade.API.Configuration
{
    public static class BuilderConfig
    {
        public static IConfigurationBuilder ConfigureDevelopmentEnvironment(this IConfigurationBuilder configuration, IWebHostEnvironment Environment)
        {
            configuration.SetBasePath(Environment.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{Environment}.json", true, true)
               .AddEnvironmentVariables();

            if (Environment.IsDevelopment())
            {
                configuration.AddUserSecrets<Program>();
            }

            return configuration;
        }
    }
}
