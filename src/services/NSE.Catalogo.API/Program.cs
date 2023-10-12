using NSE.Catalogo.API.Configuration;
using NSE.WebApi.Core.Identidade;

namespace NSE.Catalogo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.ConfigureDevelopmentEnvironment(builder.Environment);

            // Add services to the container.

            builder.Services.AddApiConfiguration();

            builder.Services.AddJwtConfiguration(builder.Configuration);

            builder.Services.AddSwaggerConfiguration();

            builder.Services.RegisterServices(builder.Configuration);
            
            builder.Services.AddMessageBusConfiguration(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwaggerConfiguration(app.Environment);

            app.UseApiConfiguration(app.Environment);

            app.Run();
        }
    }
}