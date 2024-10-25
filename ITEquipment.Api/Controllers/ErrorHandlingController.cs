using Microsoft.AspNetCore.Mvc;

namespace ITEquipment.Api.Controllers
{
    public class ErrorHandlingController : Controller
    {
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
