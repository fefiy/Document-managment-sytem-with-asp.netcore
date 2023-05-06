
using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using AFRICOMDMSWEB.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AFRICOMDMSWEB.Controllers.Admin
{
    public class UserInformationController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserInformationController(AppDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult PersonalDetail()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDetail(ApplicationUser model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userUpdate = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value);
            if(userUpdate== null)
            {
                return NotFound();
            }

            userUpdate.Name = model.Name;
            userUpdate.PhoneNumber = model.PhoneNumber;
            userUpdate.Email = model.Email;
            userUpdate.UserName = model.UserName;
            userUpdate.NormalizedUserName = model.UserName.ToUpper();

            _db.ApplicationUsers.Update(userUpdate);
            _db.SaveChanges();
            await _signInManager.RefreshSignInAsync(userUpdate);
            TempData[SD.succes] = "you update info Succesfuly";
            return RedirectToAction("Index", "Folder");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value);
            if (user != null)
            {
                if (ModelState.IsValid)
                {

                    var result = await _userManager.ChangePasswordAsync(user,
                     model.CurrentPassword, model.NewPassword);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    TempData[SD.succes] = "You Changed the password succesfuly";
                    return RedirectToAction("Index", "Folder");
                }
            }

            return View(model);
        }
    }
}
