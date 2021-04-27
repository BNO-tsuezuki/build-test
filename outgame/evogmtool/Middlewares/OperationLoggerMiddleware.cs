using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Http;

namespace evogmtool.Middlewares
{
    public class OperationLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOperationLoggerRepository _logger;

        public OperationLoggerMiddleware(
            RequestDelegate next,
            IOperationLoggerRepository logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next.Invoke(context);
                return;
            }

            var userId = context.User.Identity.IsAuthenticated
                ? (int?)int.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                : null;

            var originalBody = context.Response.Body;

            try
            {
                if (context.Request.Path.StartsWithSegments("/api/log"))
                {
                    await _next.Invoke(context);

                    _logger.Info(userId, context.Response.StatusCode, string.Empty);
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        context.Response.Body = memoryStream;

                        await _next.Invoke(context);

                        memoryStream.Position = 0;
                        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                        _logger.Info(userId, context.Response.StatusCode, responseBody);

                        memoryStream.Position = 0;
                        await memoryStream.CopyToAsync(originalBody);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(userId, (int)HttpStatusCode.InternalServerError, e);
                throw;
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
