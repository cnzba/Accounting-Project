using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        // used in conjunction with startup.cs to route all non-api related url requests
        // to the Angular client
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }
    }
}
