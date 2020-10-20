using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Domain.Entities
{
    public class Locality
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PostalCode { get; set; }

        public Province Province { get; set; }
    }
}
