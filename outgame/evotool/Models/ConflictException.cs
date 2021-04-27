using System;
using Microsoft.AspNetCore.Http;

namespace evotool.Models
{
    public class ConflictException : Exception, IHttpStatusCodeException
    {
        public int HttpStatusCode => StatusCodes.Status409Conflict;

        public ConflictException(string message) : base(message)
        { }
    }
}
