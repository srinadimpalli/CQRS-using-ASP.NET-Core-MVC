using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreFactory.CQRS.Core.Areas.Home
{
    [Menu("Welcome")]
    public class HomeController : Controller
    {
        public HomeController(ICache cache) { }

        [HttpGet("")]
        public IActionResult Welcome() => View();
    }
}
