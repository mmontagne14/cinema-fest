using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Application.Dtos
{
    public class LocalityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PostalCode { get; set; }

        public ProvinceDto Province { get; set; }
    }
}
