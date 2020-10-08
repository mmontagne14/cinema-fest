using CinemaFest.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Persistence.MockData
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IFestivalRepository festivalRepository)
        {
            Festivals = festivalRepository;
        }
        
        public IFestivalRepository Festivals { get; }

        public IEventRepository Events => throw new NotImplementedException();

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
           
        }
    }
}
