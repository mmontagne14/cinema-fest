using CinemaFest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Awards { get; set; }
        public string Rules { get; set; }
        public string Fees { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public Address Location { get; set; }

        public int FestivalId { get; set; }
        public Festival Festival { get; set; } 

        public ICollection<EventType> EventTypes { get; set; }
        public ICollection<EventCategory> Categories { get; set; }
        public ICollection<GenericDeadline> Deadlines { get; set; }
        public ICollection<EventPeople> People { get; set; }
    }
}
