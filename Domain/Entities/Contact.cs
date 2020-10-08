using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Domain.Entities
{
    public class Contact
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<SocialNetwork> SocialNetworks { get; set; }
    }
}
