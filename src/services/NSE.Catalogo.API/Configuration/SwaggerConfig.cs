using Microsoft.OpenApi.Models;

namespace NSE.Catalogo.API.Configuration;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NerdStore Enterprise API",
                Description = "Esta API faz parte do curso ASP.NET Core Enterprise Applications.",
                Contact = new OpenApiContact { Name = "Rômulo Ferreira Fraga", Email = "romulo.fraga.dev@gmail.com" },
                License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensourse.org/licenses/MIT") }
            });

            //Add JWT Header
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            //Exigir Token para metodos restritos
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app,
        IWebHostEnvironment Environment)
    {
        if (Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
        }

        return app;
    }
}