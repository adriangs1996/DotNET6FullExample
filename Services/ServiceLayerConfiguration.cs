using Microsoft.Extensions.DependencyInjection;
using Services.Contracts;
using Services.Implementation;

namespace Services
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSignalR();
            return services;
        }

        public static IServiceCollection AddClientServices(this IServiceCollection services, Uri baseAddress)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
            services.AddSingleton<IRestService, RestService>();

            return services;
        }
    }
}