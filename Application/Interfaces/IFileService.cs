using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaFest.Application.Interfaces
{
    public interface IFileService
    {
        string GetBase64FromStream(string varFilePath);
    }
}
