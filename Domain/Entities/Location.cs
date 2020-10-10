using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class Location : Address
    {
        

        public int FestivalId { get; set; }

        public int? EventId { get; set; }
    }
}
