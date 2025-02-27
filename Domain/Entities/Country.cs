﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IsoCode {get;set;}

        public ICollection<Province> Provinces { get; set; }
    }
}
