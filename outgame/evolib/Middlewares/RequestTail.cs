using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace evolib.Middlewares
{
    public class RequestTail
    {
		private readonly RequestDelegate _next;

		public RequestTail(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context

		)
		{




			////////////////////////////
			//
			await _next(context);
			//
			////////////////////////////




		}

	}
}
