
using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AFRICOMDMSWEB.Controllers.Admin
{
    public class UserProfile : Controller
    {
        private readonly AppDbContext _db;

        public UserProfile(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult PersonalDetail()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value);
            return View(user);  
        }

        public IActionResult PersonalDetail(ApplicationUser model)
        {
            return View();
        }
    }
}
