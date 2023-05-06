using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models.ViewModel;
using Dms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AFRICOMDMSWEB.Controllers.Admin
{
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _db;
        public AttendanceController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Take()
        {
            var model = new AttendanceViewModel()
            {
                SelectAttendanceActions = _db.AttendanceActions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Take(AttendanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existAttendance = _db.Atttendances.Where(a => a.DateTime.Date == model.attendances[2].DateTime.Date);
                if (existAttendance.Count()  > 0)
                {
                    TempData[SD.error] = "Attendance Fo this Day Already Taken";
                    return RedirectToAction("Index");
                }

                
                    foreach (var item in model.attendances)
                {
                        _db.Atttendances.Add(item);
                        _db.SaveChanges();
                    
                }
                
                return RedirectToAction("Index");
            }
            model.SelectAttendanceActions = _db.AttendanceActions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(model);
        }
    }
}
