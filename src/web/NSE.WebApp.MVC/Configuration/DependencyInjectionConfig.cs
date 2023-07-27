using Microsoft.AspNetCore.Mvc.DataAnnotations;
using NSE.WebApi.Core.Usuario;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

            services.AddTransient<HttpClientAuthorizationDelegateHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            services
                .AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                //.AddTransientHttpErrorPolicy(
                //    policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    policy => policy.CircuitBreakerAsync(20, TimeSpan.FromSeconds(30))
                );

            services
                .AddHttpClient("Refit", options =>
                {
                    options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
                })
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspnetUser, AspNetUser>();
        }

        public class PollyExtensions
        {
            public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
            {
                var retry = HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(new[]
                        {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10),
                        }, (outcome, timespan, retryCount, context) =>
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"Tentando pela {retryCount} vez!");
                            Console.ForegroundColor = ConsoleColor.White;
                        });
                return retry;
            }
        }
    }
}
