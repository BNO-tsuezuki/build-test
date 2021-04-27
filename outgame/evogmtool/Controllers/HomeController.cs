using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace evogmtool.Controllers
{
    [OpenApiIgnore]
    [Route("")]
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        { }

        [HttpGet("")]
        public IActionResult Home()
        {
            return Redirect("/index.html");
        }
    }
}
