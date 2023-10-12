﻿using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Configuration;

public static class WebAppConfig
{
    public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        services.Configure<AppSettings>(configuration);
    }

    public static void UseMvcConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseExceptionHandler("/erro/500");
        app.UseStatusCodePagesWithRedirects("/erro/{0}");
        app.UseHsts();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityConfiguration();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Catalogo}/{action=Index}/{id?}");
        });
    }
}