using CinemaFest.Application.Interfaces;
using CinemaFest.Domain.Entities;
using CinemaFest.Persistence.MockData.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Persistence.MockData
{
    public class FestivalRepository : IFestivalRepository
    {
        public Task<int> AddAsync(Festival entity)
        {
            //save entity
            entity.Id = FestivalFactory.GetFestivalList().FirstOrDefault().Id;
            return Task.FromResult(entity.Id);
        }

        public Task<int> CreateFestivalWithImagesAndLocationsAsync(Festival festival)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Festival>> RetrieveAllFestivalsWithImagesAndLocationsAsync()
        {
            return Task.FromResult(FestivalFactory.GetFestivalList());
        }

        public Task<Festival> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Festival entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateFestivalWithImagesAndLocationsAsync(Festival festival)
        {
            throw new NotImplementedException();
        }

        public Task<Festival> RetrieveFestivalWithImagesAndLocationsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool FestivalExistsById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
