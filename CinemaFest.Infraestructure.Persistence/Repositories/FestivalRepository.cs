using CinemaFest.Application.Interfaces;
using CinemaFest.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

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

            SqlMapper.AddTypeHandler(typeof(Contact), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(ICollection<Address>), new JsonTypeHandler());
        }

        public Task<int> AddAsync(Festival entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddFestivalAsync(Festival festival)
        {
            var insertFestivalSql = "InsertFestival";

            var festivalId = await dbConnection.ExecuteScalarAsync<int>(insertFestivalSql,
                new
                {
                    festival.CreatedAt,
                    festival.ModifiedAt,
                    festival.Name,
                    festival.About,
                    festival.FirstEditionYear,
                    festival.ProfileImg,
                    festival.CoverPageImg,
                    festival.Active,
                    festival.Contact,
                    festival.Locations
                }, dbTransaction, commandType: CommandType.StoredProcedure);

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

        public class JsonTypeHandler : SqlMapper.ITypeHandler
        {
            public void SetValue(IDbDataParameter parameter, object value)
            {
                parameter.Value = JsonConvert.SerializeObject(value);
            }

            public object Parse(Type destinationType, object value)
            {
                return JsonConvert.DeserializeObject(value as string, destinationType);
            }
        }

        
    }
}
