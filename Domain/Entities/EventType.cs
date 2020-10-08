using CinemaFest.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class EventType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SingularDescription { get; set; }
        public string PluralDescription { get; set; }


    }
}
