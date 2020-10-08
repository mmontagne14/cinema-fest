using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IFestivalRepository Festivals { get; }
        IEventRepository Events { get; }
        void Commit();
    }
}
