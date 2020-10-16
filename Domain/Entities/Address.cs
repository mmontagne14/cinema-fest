using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class Address
    {
        public string StreetAddress { get; set; }

        public string City { get; set; } //TODO add table cities

        public string State { get; set; } //TODO add table states

        public int ZipCode { get; set; }
    }
}
