using CinemaFest.Application.Interfaces;
using System.IO;

namespace CinemaFest.Infraestructure.Shared.Services
{
    public class FileService : IFileService
    {
        public byte[] GetBinaryFileFromStream(string varFilePath)
        {
            byte[] file;
            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            return file;
        }
    }
}
