using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Application.Interfaces
{
    public interface IFileService
    {
        byte[] GetBinaryFileFromStream(string varFilePath);
    }
}
