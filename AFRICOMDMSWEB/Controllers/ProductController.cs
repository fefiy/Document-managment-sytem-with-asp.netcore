//using Dms.Data;
//using Dms.Models;
//using Dms.Models.ViewModel;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Dms.Repository.IRepository;
//using Microsoft.AspNetCore.Identity;

//namespace Dms.Areas.Admin.Controllers
//{
//    [Authorize]
//    public class ProductController : Controller
//    {
//        private readonly AppDbContext _db;
//        private readonly IApplicationUserRepository _appUserRipo;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        public ProductController(AppDbContext db, IApplicationUserRepository applicationUser, IWebHostEnvironment webHostEnvironment)
//        {
//            _webHostEnvironment = webHostEnvironment;
//            _appUserRipo = applicationUser;

//            _db = db;
//        }

//        public IActionResult Index()
//        {
//            var claimsIdentity = (ClaimsIdentity)User.Identity;
//            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
//            IEnumerable<Document> result = _db.Documents.Where(u=> u.CreatedbyID == claim.Value);
//            return View(result);
//        }

//        public IActionResult Create()
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Document document, IFormFile FormFile)
//        {
//            if (ModelState.IsValid)
//            {
//                string wwwRootPath = _webHostEnvironment.WebRootPath;

//                if (FormFile != null)
//                {
//                    string fileName = Guid.NewGuid().ToString();
//                    var uploads = Path.Combine(wwwRootPath, @"document");
//                    var extension = Path.GetExtension(FormFile.FileName);

//                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
//                    {
//                        FormFile.CopyTo(fileStreams);
//                    }
//                    document.DocumentUrl = @"\document\" + fileName + extension;
//                }


//                var claimsIdentity = (ClaimsIdentity)User.Identity;
//                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
//                //save image to database.
//                using (var memoryStream = new MemoryStream())
//                {
//                    await FormFile.CopyToAsync(memoryStream);

//                    // Upload the file if less than 2 MB
//                    if (memoryStream.Length < 2097152)
//                    {
//                        //Convert the View 

//                        document.DocumentContentName = FormFile.FileName;
//                        document.DocumentContent = memoryStream.ToArray();

//                        document.DocumentContentType = FormFile.ContentType;
//                        document.CreatedbyID = claim.Value;

//                        // ProName = product.ProName

//                        _db.Documents.Add(document);

//                        await _db.SaveChangesAsync();
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("File", "The file is too large.");
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View();
//        }

//        public IActionResult DownloadImage(int id)
//        {
//            byte[] bytes;
//            string fileName, contentType;
//            var item = _db.Documents.FirstOrDefault(c => c.DocumentId == id);

//            if (item != null)
//            {
//                fileName = item.DocumentContentName;

//                contentType = item.DocumentContentType;
//                bytes = item.DocumentContent;
//                return File(bytes, contentType, fileName);
//            }
//            return Ok("Can't find the Image");
//        }

//        public IActionResult Users()
//        {
//            IEnumerable<ApplicationUser> Users = _appUserRipo.GetAll();
//            return View(Users);
//        }
//        [HttpGet]
//        public IActionResult ShareProduct(int id)
//        {
//            var claimsIdentity = (ClaimsIdentity)User.Identity;
//            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
//            IEnumerable<ApplicationUser> users = _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToList();

//            SharedVm sharedVm = new SharedVm()
//            {
//                RecieverList = users.Select(x => new SelectListItem
//                {
//                    Text = x.Name,
//                    Value = x.Id
//                }),
//                produtId = id  
//            };
//            //sharedVm.sharedFiles.DocumentId = id;
//            return View(sharedVm);
//        }
//        [HttpPost]
      
//        [ValidateAntiForgeryToken]
//        public IActionResult ShareProduct(SharedVm model)
//        {
//            //id = model.produtId;
//            var claimsIdentity = (ClaimsIdentity)User.Identity;
//            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
           
//            model.sharedFiles.DocumentId = model.produtId;
//            model.sharedFiles.SenderId = claim.Value;
//            if (ModelState.IsValid)
//            {
//                _db.SaveChanges();
//                return View("Index", "Product");
//            }
            
//           return View(model);
//        }

//        public IActionResult ShareProduct()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Delete(int id)
//        {
//            var delpro = _db.Documents.FirstOrDefault(u => u.DocumentId == id);

//            if(delpro== null)
//            {
//                return NotFound();
//            }
//            var oldimage = Path.Combine(_webHostEnvironment.WebRootPath,delpro.DocumentUrl.TrimStart('\\'));
//            if (System.IO.File.Exists(oldimage))
//            {
//                System.IO.File.Delete(oldimage);
//            }

//            _db.Documents.Remove(delpro);
//            _db.SaveChanges();
//            return RedirectToAction(nameof(Index));
//        }


//    }
//}
