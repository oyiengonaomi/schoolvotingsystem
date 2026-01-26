using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Votingsystem.Controllers
{
    [Authorize(Roles = "Voter")]
    public class VoterController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
