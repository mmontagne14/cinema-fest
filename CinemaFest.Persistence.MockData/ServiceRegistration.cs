using CinemaFest.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaFest.Persistence.MockData
{
    public static class ServiceRegistration
    {
        public static void AddMockDataSource(this IServiceCollection services)
        {
            services.AddTransient<IFestivalRepository, FestivalRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
