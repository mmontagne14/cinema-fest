using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class Address
    {
        public string Street { get; set; }
        public int Number { get; set; }
        public int CP { get; set; }
        public string Locality { get; set; }
        public string State { get; set; }
    }
}
