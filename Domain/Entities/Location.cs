using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class Location : Address
    {
        
        public int Id { get; set; }
        public int Festival_Id { get; set; }

        public int? EventId { get; set; }
    }
}
