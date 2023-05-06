using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using AFRICOMDMSWEB.Models.ViewModel;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace AFRICOMDMSWEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;
        private readonly IServer _server;
        public AccountController(IServer server,RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext db)
        {
            _server = server;
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        [HttpGet]
        public async Task<IActionResult> Register(string? returnurl = null)
        {

            ViewData["ReturnUrl"] = returnurl;
            var model = new RegisterVm()
            {
                DepartmentList= _db.Departments.Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.DepId.ToString()
                })
            };
            
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterVm model, string? returnurl = null)
        {
            returnurl = returnurl ?? Url.Content("~/Folder");
            ViewData["ReturnUrl"] = returnurl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.phoneNumber,
                   DepId = model.DepartmentID

                };
                

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var roles = await _userManager.GetRolesAsync(user); 
                    if(roles.Count() > 0)
                    {
                        return LocalRedirect(returnurl);
                    }
                    else
                    {
                       return RedirectToAction("NotInRole");
                    }
                }
                AddError(result);
            }

            model.DepartmentList = _db.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.DepId.ToString()
            });
            return View(model);
        }


        [HttpGet]
        public IActionResult LogIn(string? returnurl = null)
        {
            ViewData["ReturnURL"] = returnurl;
            return View();
        }


        //  log in

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginVm model, string? returnurl = null)
        {

            ViewData["ReturnURL"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/Folder");
            if (ModelState.IsValid)
            {
                 var user = await _userManager.FindByNameAsync(model.UserName);
               // var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "UserName doen't Registerd");
                }
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "wrong password");
                }
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Count() > 0)
                    {
                        return LocalRedirect(returnurl);
                     }
                    else
                    {
                        return RedirectToAction("NotInRole");
                    }

                }

                if (result.IsLockedOut)
                {
                    return RedirectToAction("LockOut");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Log in Failed");
                    return View(model);
                }

            }
            return View(model);
        }

        public IActionResult NotInRole()
        {
            return View();
        }
        // log out
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Folder");

        }


        private void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);

            }
        }


    }
}
