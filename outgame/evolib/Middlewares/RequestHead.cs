using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using evolib.Log;

namespace evolib.Middlewares
{
	public class RequestHead
	{
		private readonly RequestDelegate _next;

		public RequestHead(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync( HttpContext context
			,ILogObj logObj
		){
			var log = new LogModels.HttpRequest
			{
				RequestDate = DateTime.UtcNow,
				Url = context.Request.Path,
				TraceId = context.TraceIdentifier,
				RemoteIp = context.Connection.RemoteIpAddress.ToString(),
			};

			logObj.AddChild(log);
	

			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			var responseBodyOrg = context.Response.Body;
			var responseBody = new System.IO.MemoryStream();
			context.Response.Body = responseBody;

			////////////////////////////
			//
			await _next(context);
			//
			////////////////////////////

			var bytes = new byte[responseBody.Length];
			responseBody.Position = 0;
			await responseBody.ReadAsync(bytes, 0, (int)responseBody.Length);
			//var response = System.Text.Encoding.UTF8.GetString(bytes);
			var response = (1000000 < responseBody.Length)
				?"Interim measure for error."
				:System.Text.Encoding.UTF8.GetString(bytes);
			responseBody.Position = 0;
			await responseBody.CopyToAsync(responseBodyOrg);

			sw.Stop();

			log.Response = response;
			log.StatusCode = context.Response.StatusCode;
			log.ElapsedTime = sw.ElapsedMilliseconds;
			log.ResponseDate = DateTime.UtcNow;
		}
	}
}
