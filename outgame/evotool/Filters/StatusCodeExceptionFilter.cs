using System.Threading.Tasks;
using evotool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace evotool.Filters
{
    public class StatusCodeExceptionFilter : ExceptionFilterAttribute, IAsyncExceptionFilter
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is IHttpStatusCodeException)
            {
                var message = new ErrorMessageDto(context.Exception.Message);

                context.Result = new ObjectResult(message)
                {
                    StatusCode = (context.Exception as IHttpStatusCodeException).HttpStatusCode
                };

                context.ExceptionHandled = true;
            }

            return Task.CompletedTask;
        }
    }
}
