using NSE.Carrinho.API.Configuration;
using NSE.WebApi.Core.Identidade;

namespace NSE.Carrinho.API
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

            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.RegisterServices(builder.Configuration);
            
            builder.Services.AddSwaggerConfiguration();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}