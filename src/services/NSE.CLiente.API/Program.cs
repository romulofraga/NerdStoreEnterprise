using MediatR;
using NSE.Clientes.API.Configuration;
using NSE.WebApi.Core.Identidade;

namespace NSE.CLientes.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.ConfigureDevelopmentEnvironment(builder.Environment);

            // Add services to the container.

            builder.Services.AddApiConfiguration(builder.Configuration);

            builder.Services.AddJwtConfiguration(builder.Configuration);

            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddMediatR(typeof(Program));

            builder.Services.RegisterServices();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwaggerConfiguration(app.Environment);

            app.UseApiConfiguration(app.Environment);

            app.Run();
        }
    }
}