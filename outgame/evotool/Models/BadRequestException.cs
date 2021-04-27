using System;
using Microsoft.AspNetCore.Http;

namespace evotool.Models
{
    public class BadRequestException : Exception, IHttpStatusCodeException
    {
        public int HttpStatusCode => StatusCodes.Status400BadRequest;

        public BadRequestException(string message) : base(message)
        { }
    }
}
