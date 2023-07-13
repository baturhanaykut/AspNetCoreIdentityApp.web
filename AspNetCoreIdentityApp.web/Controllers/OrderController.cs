using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.web.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Policy = "OrderPermissionReadOrDeletePolicy")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
