

namespace CinemaFest.Application.Dtos
{
    public class AddressDto { 

        public string Street { get; set; }

        public string Number { get; set; }

        public int? Floor { get; set; }

        public string Apartment { get; set; }

        public LocalityDto Locality { get; set; }

        public int Locality_Id { get; set; }
    }
}
