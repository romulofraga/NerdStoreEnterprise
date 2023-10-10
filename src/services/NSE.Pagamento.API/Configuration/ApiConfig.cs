using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Facade;
using NSE.Pagamentos.API.Data;
using NSE.WebApi.Core.Identidade;

namespace NSE.Pagamentos.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PagamentosContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));

            services.AddControllers();

            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        "Total",
                        builder =>
                        {
                            builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
                });

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment Environment)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            return app;
        }
    }
}
