namespace NSE.Pedidos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            //services.AddScoped<IMediatorHandler, MediatorHandler>();
            //services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            //services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            //services.AddScoped<IClienteRepository, ClienteRepository>();
            //services.AddScoped<ClientesContext>();

            return services;
        }
    }
}
