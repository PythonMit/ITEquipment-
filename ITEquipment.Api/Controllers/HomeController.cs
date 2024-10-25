using Microsoft.AspNetCore.Mvc;

namespace ITEquipment.Api.Controllers
{
    public class HomeController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Main()
        {
            return View();
        }
    }
}
