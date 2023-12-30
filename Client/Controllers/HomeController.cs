using Client.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            ViewBag.Components = components;
            return View();
        }
    }
}
