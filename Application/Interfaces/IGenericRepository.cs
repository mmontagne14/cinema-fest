using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaFest.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllFestivalsAsync();
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}
