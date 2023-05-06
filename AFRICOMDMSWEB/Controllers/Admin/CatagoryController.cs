
using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using AFRICOMDMSWEB.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AFRICOMDMSWEB.Controllers.Admin
{
    public class CatagoryController : Controller
    {
        private readonly AppDbContext _db;
        public CatagoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

           List<Category> categories = GetSubCategory(_db.Catagories.ToList(), 1);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CatagoryVm vm = new CatagoryVm()
            {
                subCatagories = _db.Catagories.Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString(),

                }),
                 
                
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CatagoryVm model)
        {
            if (ModelState.IsValid)
            {
                if (model.category.ParentCategoryId == null)
                {
                    model.category.ParentCategoryId = 0;
                }
                _db.Catagories.Add(model.category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        } 


        private  List<Category> GetSubCategory(List<Category> items, int parentId)
        {
            return items.Where(x => x.ParentCategoryId == parentId)
                .Select(e => new Category
                {
                    Id = e.Id,
                    Title = e.Title,
                    ParentCategoryId = e.ParentCategoryId,
                    SubCategories = GetSubCategory(items, e.Id)
                                 
                }).ToList();
        }


       
    }
}
