using CinemaFest.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaFest.Application.Interfaces
{
    public interface IFestivalRepository // : IGenericRepository<Festival>
    {
        Task<int> CreateFestivalWithImagesAndLocationsAsync(Festival festival);
        Task<int> UpdateFestivalWithImagesAndLocationsAsync(Festival festival);
        Task<IEnumerable<Festival>> RetrieveAllFestivalsWithImagesAndLocationsAsync();
        Task<Festival> RetrieveFestivalWithImagesAndLocationsByIdAsync(int id);
    }
}
