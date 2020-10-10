using CinemaFest.Application.Interfaces;
using CinemaFest.Infraestructure.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaFest.Infraestructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddShared(this IServiceCollection services)
        {
            services.AddTransient<IFileService, FileService>();
        }
    }
}
