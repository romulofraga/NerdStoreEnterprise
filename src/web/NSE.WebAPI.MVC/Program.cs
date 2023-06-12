using NSE.WebAPI.MVC.Configuration;

namespace NSE.WebAPI.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddIdentityConfiguration();

            // Add services to the container.
            builder.Services.AddMvcConfiguration();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMvcConfiguration(app.Environment);

            app.Run();
        }
    }
}