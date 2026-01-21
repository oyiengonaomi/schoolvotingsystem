using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Votingsystem.Models;
using Votingsystem.ViewModels;

namespace Votingsystem.Controllers
{
    public class AccountController : Controller
    {
        //Injecting the user_manager and _signinmanager
        // 1. Declare the private fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //2.Inject them through the constructor
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Map the ViewModel data to the ApplicationUserModel
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                //Create the user in the database
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //If successful, sign them in and redirect to Home
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                // if identity has errors e.g password too weak, add them to the screen
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded) return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }
    }
}
