using NSE.Carrinho.API.Configuration;
using NSE.WebApi.Core.Identidade;

namespace NSE.Carrinho.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.ConfigureDevelopmentEnvironment(builder.Environment);

        // Add services to the container.

        builder.Services.AddApiConfiguration();

        builder.Services.AddJwtConfiguration(builder.Configuration);

        builder.Services.RegisterServices(builder.Configuration);

        builder.Services.AddSwaggerConfiguration();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseSwaggerConfiguration(app.Environment);

        app.UseSwaggerConfiguration(app.Environment);

        app.Run();
    }
}