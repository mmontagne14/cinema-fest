using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string message = null)
        {
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; }
        public string Message { get; set; }

        public List<string> Error { get; set; }
        public T Data { get; set; }
    }
}