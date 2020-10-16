using CinemaFest.Application.Interfaces;
using System;
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
    }
}
