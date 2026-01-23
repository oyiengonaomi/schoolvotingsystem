using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
        public IActionResult Register()
        {

            return View(new RegisterViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Map the ViewModel data to the ApplicationUserModel
                var user = new ApplicationUser { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                //Create the user in the database
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
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
        public IActionResult Login()
        {
            
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Adds security against cross-site attacks
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // PasswordSignInAsync handles the hashing and checking for you
                // lockoutOnFailure: true is a good security practice for the future
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // If the login is successful, send them to the Home page
                    return RedirectToAction("Index", "Home");
                }

                // If it fails, add a generic error message
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
            }

            // If we got this far, something failed; redisplay the form with the user's email still there
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
