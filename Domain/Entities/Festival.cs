using CinemaFest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CinemaFest.Domain.Entities
{
    public class Festival : BaseEntity
    {
        public string Name { get; set; }

        public string About { get; set; }
        
        //public string ProfileImg { get; set; }

        //public string CoverPageImg { get; set; }
        public int FirstEditionYear { get; set; }

        public bool Active { get; set; }
        public Contact Contact { get; set; }

        public ICollection<Location> Locations { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<FestivalImage> Images { get; set; }


        public void SetInitialProperties()
        {
            this.CreatedAt = DateTime.Now;
            this.ModifiedAt = DateTime.Now;
            this.Active = true;
        }
    }
}
