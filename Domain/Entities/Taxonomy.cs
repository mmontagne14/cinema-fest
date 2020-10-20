using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Domain.Entities
{
    public class Taxonomy
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TaxonomyType_Id { get; set; }

        public int? Festival_Id { get; set; }


    }
}
