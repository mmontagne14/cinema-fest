

namespace CinemaFest.Persistence.Dapper.Repositories
{
    using CinemaFest.Application.Interfaces;
    using CinemaFest.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    public class EventRepository : IEventRepository
    {
        private readonly IDbTransaction dbTransaction;
        private readonly IDbConnection dbConnection;
        private readonly string insertRowRetrievalQuery;
        public EventRepository(IDbTransaction dbTransaction, string insertRowRetrievalQuery)
        {
            this.dbTransaction = dbTransaction;
            this.insertRowRetrievalQuery = insertRowRetrievalQuery;
            this.dbConnection = dbTransaction.Connection;
        }

        public Task<int> AddAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetAllFestivalsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}
