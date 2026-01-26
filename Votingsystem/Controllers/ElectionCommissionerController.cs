using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Votingsystem.Controllers
{
    [Authorize(Roles = "Election Commissioner")]
    public class ElectionCommissionerController : Controller
    {
        public IActionResult Dashboard()
        {
            
            return View();
        }
    }
}
