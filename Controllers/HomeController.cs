
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
