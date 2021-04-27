using evogmtool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace evogmtool.Controllers
{
    [OpenApiIgnore]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        //private readonly ILogger _logger;

        //public ErrorController(ILogger<ErrorController> logger)
        public ErrorController()
        {
            //_logger = logger;
        }

        [HttpGet("{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            HttpContext.Response.StatusCode = statusCode;

            var error = new Error();
            error.StatusCode = statusCode;

            // todo: エラー表示の内容を検討する
            switch (HttpContext.Response.StatusCode)
            {
                case StatusCodes.Status403Forbidden:
                    error.Message = "Forbidden";
                    break;
                case StatusCodes.Status404NotFound:
                    error.Message = "Not Found";
                    break;
                case StatusCodes.Status500InternalServerError:
                    error.Message = "Internal Server Error";
                    break;
                default:
                    error.Message = "Error";
                    break;
            }

            return View(error);
        }
    }
}
