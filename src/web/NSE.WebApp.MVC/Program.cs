using NSE.WebApp.MVC.Configuration;

namespace NSE.WebApp.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.ConfigureDevelopmentEnvironment(builder.Environment);

            builder.Services.AddIdentityConfiguration();

            // Add services to the container.
            builder.Services.AddMvcConfiguration(builder.Configuration);

            builder.Services.RegisterServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMvcConfiguration(app.Environment);

            app.UseGlobalizationConfiguration();

            app.Run();
        }
    }
}