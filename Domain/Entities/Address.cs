using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class Address
    {
        public string Street { get; set; }

        public string Number { get; set; } 

        public int? Floor { get; set; }

        public string Apartment { get; set; } 
         
        public Locality Locality { get; set; }
        public int Locality_Id { get; set; }
    }
}
