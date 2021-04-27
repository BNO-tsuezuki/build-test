using System;
using Microsoft.AspNetCore.Http;

namespace evotool.Models
{
    public class NotFoundException : Exception, IHttpStatusCodeException
    {
        public int HttpStatusCode => StatusCodes.Status404NotFound;

        public NotFoundException(string message) : base(message)
        { }
    }
}
