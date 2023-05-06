
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using Dms;
using AFRICOMDMSWEB.Models.ViewModel;

namespace AFRICOMDMSWEB .Controllers.Admin
{
    public class ManageUsersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public ManageUsersController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userList = _db.ApplicationUsers.Include(u => u.Department).ToList().OrderByDescending(u=> u.Id);
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList) // 4 user
            {
                var role = userRoles.FirstOrDefault(u => u.UserId == user.Id);  // select the value based on the condition like filter in js
                if (role == null)   // if the condition is false
                {
                    user.Role = "Unsigned";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }
            }
            return View(userList);
        }

        public IActionResult Edit(string userId)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            var userRoles = _db.UserRoles.ToList();
            if (objFromDb == null)
            {
                return NotFound();
            }
            var userRole = _db.UserRoles.FirstOrDefault(u => u.UserId == userId);
            if (userRole != null)
            {
                objFromDb.Role = _db.Roles.FirstOrDefault(u => u.Id == userRole.RoleId).Id;

            }
            objFromDb.RoleList = _db.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            });

            objFromDb.DepartmentList = _db.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.DepId.ToString(),

            });
            return View(objFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == user.Id);
                if (userFromDb == null)
                {
                    return NotFound();
                }
                var UserRole = _db.UserRoles.FirstOrDefault(u => u.UserId == userFromDb.Id);

                if (UserRole != null)
                {
                    var previousRole = _db.Roles.FirstOrDefault(u => u.Id == UserRole.RoleId).Name;
                    await _userManager.RemoveFromRoleAsync(userFromDb, previousRole);

                }
                await _userManager.AddToRoleAsync(userFromDb, _db.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name);
                userFromDb.DepId = user.DepId;
                _db.ApplicationUsers.Update(userFromDb);
                _db.SaveChanges();
                TempData[SD.succes] = "User Updated Succesfuly";
                return RedirectToAction(nameof(Index));
            }

            user.RoleList = _db.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            });

            return View(user);
        }

        [HttpPost]
        public IActionResult LockUnlock(string userId)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is locked and will remain locked untill lockoutend time
                //clicking on this action will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
                TempData[SD.succes] = "User unlocked successfully.";
            }
            else
            {
                //user is not locked, and we want to lock the user
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                TempData[SD.succes] = "User locked successfully.";
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimStore.claimsList)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel userClaimsViewModel)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                TempData[SD.error] = "Error while removing claims";
                return View(userClaimsViewModel);
            }

            result = await _userManager.AddClaimsAsync(user,
                userClaimsViewModel.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.IsSelected.ToString()))
                );

            if (!result.Succeeded)
            {
                TempData[SD.error] = "Error while adding claims";
                return View(userClaimsViewModel);
            }

            TempData[SD.succes] = "Claims updated successfully";
            return RedirectToAction(nameof(Index));
        }


    }
}
