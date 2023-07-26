
using NSE.Identidade.API.Configuration;

namespace NSE.Identidade.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.ConfigureDevelopmentEnvironment(builder.Environment);

            /*
             * substitui -> {
             
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment}.json", true, true)
                .AddEnvironmentVariables();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }
             */

            builder.Services.AddIdentityConfiguration(builder.Configuration);

            /*
             * substitui {
            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //JWT CONFIGURATION
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Emissor,
                    ValidIssuer = appSettings.ValidoEm
                };
            });
            // END JWT CONFIGURATION
             */

            builder.Services.AddApiConfiguration();

            /* 
             * substitui -> builder.Services.AddControllers(); 
             */

            builder.Services.AddMessageBusConfiguration(builder.Configuration);

            builder.Services.AddSwaggerConfiguration();

            /*
             * substitui {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NerdStore Enterprise API",
                    Description = "Esta API faz parte do curso ASP.NET Core Enterprise Applications.",
                    Contact = new OpenApiContact() { Name = "Rômulo Ferreira Fraga", Email = "romulo.fraga.dev@gmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensourse.org/licenses/MIT") }
                })
            );
             */

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwaggerConfiguration(app.Environment);

            /*
             * substitui por -> {
             
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
             }

             }
             */

            /*
            app.UseAuthorization();

            app.UseAuthentication();

             * substituido por -> {
             
             app.UseIdentityConfiguration();
             
             Dentro de ApiConfig.cs
            }
            */

            app.UseApiConfiguration(app.Environment);

            /*
             * substitui {

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseAuthentication();


            app.MapControllers();

            }
            */

            app.Run();
        }
    }
}