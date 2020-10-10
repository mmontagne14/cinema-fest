namespace CinemaFest.Persistence.Dapper
{
    using CinemaFest.Application.Interfaces;
    using System;
    using System.Data;
    using Microsoft.Extensions.Configuration;
    using MySql.Data.MySqlClient;
    using CinemaFest.Persistence.Dapper.Repositories;
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection dbConnection;
        private readonly IDbTransaction dbTransaction;
        private readonly IConfiguration configuration;
        public UnitOfWork(IConfiguration configuration)
        {
            string rowInsertRetrievalQuery;

            this.configuration = configuration;

            dbConnection = new MySqlConnection(configuration.GetConnectionString("MySqlServerConnection"));
            rowInsertRetrievalQuery = "; SELECT  last_insert_id();";
            dbConnection.Open();
            dbTransaction = dbConnection.BeginTransaction();

            Festivals = new FestivalRepository(dbTransaction, rowInsertRetrievalQuery);
            Events = new EventRepository(dbTransaction, rowInsertRetrievalQuery);
        }
        public IFestivalRepository Festivals { get; set; }

        public IEventRepository Events { get; set; }

        public void Commit()
        {
            try
            {
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not commit the transaction, reason: {ex.Message}");
                dbTransaction.Rollback();
            }
            finally
            {
                dbTransaction.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);

        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbTransaction?.Dispose();
                dbConnection?.Dispose();
            }
        }
    }
}
