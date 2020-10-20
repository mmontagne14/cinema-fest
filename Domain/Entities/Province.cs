using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Domain.Entities
{
    public class Province
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IsoCode { get; set; }

        public Country Country { get; set; }

        public ICollection<Locality> Localities { get; set; }
    }
}
