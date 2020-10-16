using CinemaFest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Application.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }
        public ImageTypes Type { get; set; }

        public string FilePath { get; set; }


    }
}
