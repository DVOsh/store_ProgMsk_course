using Microsoft.AspNetCore.Mvc;

namespace Store.YandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        // /YandexKassa/Home/Callback
        public IActionResult Callback()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
