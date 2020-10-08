using CinemaFest.Application.Interfaces;
using CinemaFest.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace CinemaFest.Persistence.Dapper.Repositories
{
    public class FestivalRepository : IFestivalRepository
    {
        private readonly IDbTransaction dbTransaction;
        private readonly IDbConnection dbConnection;
        private readonly string insertRowRetrievalQuery;

        public FestivalRepository(IDbTransaction dbTransaction, string insertRowRetrievalQuery)
        {
            this.dbTransaction = dbTransaction;
            this.insertRowRetrievalQuery = insertRowRetrievalQuery;
            this.dbConnection = dbTransaction.Connection;
        }
        public async Task<int> AddAsync(Festival entity)
        {
            var insertFestivalSql = new StringBuilder(@"
                INSERT INTO festivals(CreatedAt,ModifiedAt,Name,About,FirstEditionYear,ProfileImg,CoverageImg,Active)
                values(@createdAt,@modifiedAt,@name,@about,@firstEditionYear,@ProfileImg,@CoverPageImg,@active)");

            var festivalId = await dbConnection.ExecuteScalarAsync<int>(
                insertFestivalSql.Append(insertRowRetrievalQuery)
                .ToString(),
                new
                {
                    entity.CreatedAt,
                    entity.ModifiedAt,
                    entity.Name,
                    entity.About,
                    entity.FirstEditionYear,
                    entity.ProfileImg,
                    entity.CoverPageImg,
                    entity.Active
                }, dbTransaction);

            return festivalId;
                
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Festival>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Festival> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Festival entity)
        {
            throw new NotImplementedException();
        }
    }
}
