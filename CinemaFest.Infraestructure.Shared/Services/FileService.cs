using CinemaFest.Application.Interfaces;
using CinemaFest.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CinemaFest.Infraestructure.Shared.Services
{
    public class FileService : IFileService
    {
        public string GetBase64FromStream(string varFilePath)
        {
            byte[] file;
            string fileBase64;
            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                    fileBase64 = Convert.ToBase64String(file);
                }
            }
            return fileBase64;
        }

        public ICollection<FestivalImage> GetBase64FromFestivalImagesStream(ICollection<FestivalImage> images)
        {
            foreach (FestivalImage img in images)
            {
                img.Img = GetBase64FromStream(img.FilePath);
            }

            return images;
        }
    }
}
