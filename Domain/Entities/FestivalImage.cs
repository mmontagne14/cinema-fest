using CinemaFest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Domain.Entities
{
    public class FestivalImage
    {
        public int Id { get; set; }
        public string Img { get; set; }

        public ImageTypes Type { get; set; }

        public int Festival_Id { get; set; }

        public string FilePath { get; set; }



        
    }
}
