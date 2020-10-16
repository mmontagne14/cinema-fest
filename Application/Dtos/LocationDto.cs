using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Application.Dtos
{
    public class LocationDto : AddressDto
    {
        public int Festival_Id { get; set; }

        public int? Event_Id { get; set; }
    }
}
