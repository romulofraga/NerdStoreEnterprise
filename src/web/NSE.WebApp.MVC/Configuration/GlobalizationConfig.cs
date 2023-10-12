﻿using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace NSE.WebApp.MVC.Configuration;

public static class GlobalizationConfig
{
    public static IApplicationBuilder UseGlobalizationConfiguration(this IApplicationBuilder app)
    {
        var supportedCultures = new[]
        {
            new CultureInfo("pt-BR")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        app.UseRequestLocalization(localizationOptions);

        CultureInfo.DefaultThreadCurrentCulture = supportedCultures[0];
        CultureInfo.DefaultThreadCurrentUICulture = supportedCultures[0];

        return app;
    }
}