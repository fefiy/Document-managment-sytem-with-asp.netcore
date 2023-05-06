
using AFRICOMDMSWEB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AFRICOMDMSWEB.Controllers.Admin
{
    public class RolesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(AppDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();

            return View(roles);
        }
        [HttpGet]
        public IActionResult Upsert(string? id)
        {
            if(id == null)
            {
                return View();
            }
            else
            {
                var user = _db.Roles.FirstOrDefault(u=> u.Id == id);
                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole model)
        {

            if(await _roleManager.RoleExistsAsync(model.Name))
            {
                TempData[SD.error] = "Role Already Exist";
                return RedirectToAction("Index");

            }

            if (String.IsNullOrEmpty(model.Id))
            {
                // there is no id 

                await _roleManager.CreateAsync(new IdentityRole() { Name = model.Name });
                TempData[SD.succes] = "ROle created successfuly";
            }
            else 
            {
                //update
                var objRoleFromDb = _db.Roles.FirstOrDefault(u => u.Id == model.Id);
                if (objRoleFromDb == null)
                {
                    TempData[SD.error] = "Role not found.";
                    return RedirectToAction(nameof(Index));
                }
                objRoleFromDb.Name = model.Name;
                objRoleFromDb.NormalizedName = model.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(objRoleFromDb);
                TempData[SD.succes] = "Role updated successfully";
            }
            return RedirectToAction("Index");   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(string id)
        {
            var fromdb = _db.Roles.FirstOrDefault(u => u.Id == id);
            if (fromdb == null)
            {
                TempData[SD.error] = "Role is not found";
            }
            var usersofthisrole = _db.UserRoles.Where(u => u.RoleId == id).Count();
            if(usersofthisrole > 0)
            {
                TempData[SD.error] = "can not delete these role because users are asigned to it";
                return RedirectToAction(nameof(Index));
            }
           
            await _roleManager.DeleteAsync(fromdb);
            TempData[SD.succes] = "Role Deleted successfuly";
            return RedirectToAction(nameof(Index));
        }

    }
}
