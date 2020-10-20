using CinemaFest.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaFest.Application.Interfaces
{
    public interface IFestivalRepository // : IGenericRepository<Festival>
    {
        Task<int> CreateFestivalAsync(Festival festival);
        Task<int> UpdateFestivalWithImagesAndLocationsAsync(Festival festival);
        Task<IEnumerable<Festival>> RetrieveAllFestivalsWithImagesAndLocationsAsync();
        Task<Festival> RetrieveFestivalByIdAsync(int id);

        bool FestivalExistsById(int Id);
    }
}
