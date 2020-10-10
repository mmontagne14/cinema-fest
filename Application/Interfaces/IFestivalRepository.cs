using CinemaFest.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaFest.Application.Interfaces
{
    public interface IFestivalRepository : IGenericRepository<Festival>
    {
        Task<int> AddFestivalAsync(Festival festival);
    }
}
