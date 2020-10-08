using CinemaFest.Domain.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace CinemaFest.Domain.Entities
{
    public class Festival : BaseEntity
    {
        public string Name { get; set; }

        public string About { get; set; }
        
        public byte[] ProfileImg { get; set; }

        public byte[] CoverPageImg { get; set; }
        public int FirstEditionYear { get; set; }

        public bool Active { get; set; }
        public Contact Contact { get; set; }

        public ICollection<Address> FestivalLocations { get; set; }

        public ICollection<Event> Events { get; set; }

    }
}
