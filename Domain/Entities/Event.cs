﻿using CinemaFest.Domain.Common;
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

        public DateTime EventDate { get; set; }
        public Location Location { get; set; }

        public int FestivalId { get; set; }
        public Festival Festival { get; set; } 

        public ICollection<Taxonomy> Taxonomies { get; set; }
        public ICollection<EventPeople> People { get; set; }
    }
}
