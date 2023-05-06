
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models.ViewModel;
using AFRICOMDMSWEB.Models;

namespace AFRICOMDMSWEB.Controllers.Admin
{
    public class FileSharededController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor conxt;
        public FileSharededController(AppDbContext db, IHttpContextAccessor httpContext)
        {
            conxt = httpContext;
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> ShareProduct(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<ApplicationUser> users = _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToList();
            conxt.HttpContext.Session.SetInt32("shareId", id);

            var sharedVM = new SharedVm()
            {
                RecieverList = users.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                }),
                produtId = id,

                Depatment = _db.Departments.ToList(),

            };

            return View(sharedVM);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShareProduct(SharedVm model)
        {
            //id = model.produtId;
            int IsUpdate = 0;
            int IsDwloadable = 0;
            int IsSharedable = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string pubpri = model.sharedFiles.IspublicORPrivate;


            if (model.IsSharedable == true)
            {
                IsSharedable = 1;
            }
            if (model.IsUpdatable == true)
            {
                IsUpdate = 1;
            }
            if (model.Isdowloadble)
            {
                IsDwloadable = 1;
            }
            List<FileShared> Files = new List<FileShared>();
            List<shareFolder> folders = new List<shareFolder>();
            var FolderId = _db.Documents.FirstOrDefault(d => d.DocumentId == model.produtId).FolderId;
            var Folder = _db.Folders.FirstOrDefault(f => f.FolderId == FolderId);
            var SubFol = GetSubFolder(Folder);

            if (model.sharedFiles.IspublicORPrivate == "public")
            {
                var users = _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToList();


                foreach (var user in users)
                {
                    if (SubFol.Count() > 0)
                    {
                        foreach (var subFol in SubFol)
                        {
                            var fold = new shareFolder()
                            {
                                ReceiverId = user.Id,
                                SenderId = claim.Value,
                                FolderId = subFol.FolderId,
                           
                            };
                            var shareF = _db.ShareFolders.Where(f => f.FolderId == fold.FolderId && f.ReceiverId == fold.ReceiverId && f.SenderId == fold.SenderId);
                            if (shareF.Count() < 1)
                            {
                                folders.Add(fold);
                            }
                        }

                    }
                    Files.Add(new FileShared
                    {
                        ReceiverId = user.Id,
                        SenderId = claim.Value,
                        DocumentId = model.produtId,
                        FileSentAt = DateTime.Now,
                        Isdowloadble = IsDwloadable,
                        IsUpdatable = IsUpdate,
                        IsSharedable = IsSharedable,
                    });
                    if (model.IsSharedable == true)
                    {

                    }
                }
                _db.ShareFolders.AddRange(folders);
                _db.FileShares.AddRange(Files);
                _db.SaveChanges();
                TempData[SD.succes] = "You Shared the file sucessfuly";
                return RedirectToAction("Index", "Folder");

            }
            if (model.sharedFiles.IspublicORPrivate == "private")
            {
             
                if (SubFol.Count() > 0)
                {
                    // var foolders = new List<Folder>();
                    foreach (var subFol in SubFol)
                    {

                        var fold = new shareFolder()
                        {
                            ReceiverId = model.sharedFiles.ReceiverId,
                            SenderId = claim.Value,
                            FolderId = subFol.FolderId,
                            //FileSentAt = DateTime.Now,
                          //  IsSharedable = IsSharedable,

                        };
                        var shareF = _db.ShareFolders.Where(f=> f.FolderId == fold.FolderId && f.ReceiverId== fold.ReceiverId && f.SenderId == fold.SenderId);
                        if(shareF.Count() < 1){
                            folders.Add(fold);
                        }
                       
                    }
                };

                model.sharedFiles.DocumentId = model.produtId;
                model.sharedFiles.SenderId = claim.Value;
                model.sharedFiles.FileSentAt = DateTime.Now;
                model.sharedFiles.Isdowloadble = IsDwloadable;
                model.sharedFiles.IsSharedable = IsSharedable;
                model.sharedFiles.IsUpdatable = IsUpdate;
                if (ModelState.IsValid)
                {
                    _db.ShareFolders.AddRange(folders);
                    _db.FileShares.Add(model.sharedFiles);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Folder");
                }
            }
            if(model.sharedFiles.IspublicORPrivate == "usergroup")
            {

                var users = _db.ApplicationUsers.Where(u => u.DepId == model.DepartmetId && u.Id != claim.Value).ToList();
                if (users == null)
                {
                    TempData["error"] = "NO User is Assigned in this Department";
                    return RedirectToAction("Index", "Folder");
                }
                foreach (var user in users)
                {
                    if (SubFol.Count() > 0)
                    {
                    foreach (var subFol in SubFol)
                        {
                          var  fold =new shareFolder()
                            {
                                ReceiverId = user.Id,
                                SenderId = claim.Value,
                                FolderId = subFol.FolderId,
                                //FileSentAt = DateTime.Now,
                              //  IsSharedable = IsSharedable,
                            };
                            var shareF = _db.ShareFolders.Where(f => f.FolderId == fold.FolderId && f.ReceiverId == fold.ReceiverId && f.SenderId == fold.SenderId);
                            if (shareF.Count() < 1)
                            {
                                folders.Add(fold);
                            }
                        }
                    
                    }
                      
                    Files.Add(new FileShared
                    {
                        ReceiverId = user.Id,
                        SenderId = claim.Value,
                        DocumentId = model.produtId,
                        FileSentAt = DateTime.Now,
                        Isdowloadble = IsDwloadable,
                        IsUpdatable = IsUpdate,
                        IsSharedable = IsSharedable,

                    });
                }

                _db.FileShares.AddRange(Files);
                _db.SaveChanges();
                TempData[SD.succes] = "You Shared the file sucessfuly";
                return RedirectToAction("Index", "Folder");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult FileRecieved()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<FileShared> recieve = _db.FileShares.Where(f => f.ReceiverId == claim.Value).Include(u => u.Sender).Include(u => u.Document).ToList().OrderByDescending(o => o.Id);
            return View(recieve);
        }
        [HttpGet]
        public IActionResult FileSended()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<FileShared> send = _db.FileShares.Where(f => f.SenderId == claim.Value).Include(u => u.Receiver).Include(u => u.Document).ToList().OrderByDescending(o => o.Id);
            return View(send);
        }



        [HttpGet]
        public IActionResult ManageShared(int id)
        {
            var sharedDoc = _db.FileShares.FirstOrDefault(s => s.Id == id);
            bool update = false;
            bool shared = false;
            bool download = false;

            if (sharedDoc.IsSharedable == 1)
            {
                shared = true;
            }
            if (sharedDoc.Isdowloadble == 1)
            {
                download = true;
            }
            if (sharedDoc.IsUpdatable == 1)
            {
                update = true;
            }

            var model = new SharedVm()
            {
                IsUpdatable = update,
                Isdowloadble = download,
                IsSharedable = shared,
                produtId = id,

            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ManageShared(SharedVm model)
        {
            var id = model.produtId;

            var SharedDoc = _db.FileShares.FirstOrDefault(s => s.Id == id);
            if (model.IsSharedable)
            {
                SharedDoc.IsSharedable = 1;
            }
            else
            {
                SharedDoc.IsSharedable = 0;
            }
            if (model.Isdowloadble)
            {
                SharedDoc.Isdowloadble = 1;
            }
            else
            {
                SharedDoc.Isdowloadble = 0;

            }
            if (model.IsUpdatable)
            {
                SharedDoc.IsUpdatable = 1;
            }
            else
            {
                SharedDoc.IsUpdatable = 0;
            }
            _db.FileShares.Update(SharedDoc);
            _db.SaveChanges();
            return RedirectToAction(nameof(FileSended));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var shareDoc = _db.FileShares.FirstOrDefault(s => s.Id == id);

            if (shareDoc == null)
            {
                return NotFound();
            }
            _db.FileShares.Remove(shareDoc);
            _db.SaveChanges();
            return RedirectToAction(nameof(FileSended));
        }

        public async Task<IActionResult> ShareProductShared(int id)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<ApplicationUser> users = _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToList();
            conxt.HttpContext.Session.SetInt32("shareId", id);

            var sharedVM = new SharedVm()
            {
                RecieverList = users.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                }),
                produtId = id,

                Depatment = _db.Departments.ToList(),

            };

            return View(sharedVM);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShareProductShared(SharedVm model)
        {
            //id = model.produtId;
            int IsUpdate = 0;
            int IsDwloadable = 0;
            int IsSharedable = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string pubpri = model.sharedFiles.IspublicORPrivate;





            if (model.sharedFiles.IspublicORPrivate == "public")
            {
                var users = _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToList();

                List<FileShared> Files = new List<FileShared>();

                foreach (var user in users)
                {
                    Files.Add(new FileShared
                    {
                        ReceiverId = user.Id,
                        SenderId = claim.Value,
                        DocumentId = model.produtId,
                        FileSentAt = DateTime.Now,
                        Isdowloadble = IsDwloadable,
                        IsUpdatable = IsUpdate,
                        IsSharedable = IsSharedable,


                    });
                    if (model.IsSharedable == true)
                    {

                    }
                }

                _db.FileShares.AddRange(Files);
                _db.SaveChanges();
                TempData[SD.succes] = "You Shared the file sucessfuly";
                return RedirectToAction("Index", "Folder");

            }
            if (model.sharedFiles.IspublicORPrivate == "private")
            {
                model.sharedFiles.DocumentId = model.produtId;
                model.sharedFiles.SenderId = claim.Value;
                model.sharedFiles.FileSentAt = DateTime.Now;
                model.sharedFiles.Isdowloadble = IsDwloadable;
                model.sharedFiles.IsSharedable = IsSharedable;
                model.sharedFiles.IsUpdatable = IsUpdate;
                if (ModelState.IsValid)
                {
                    _db.FileShares.Add(model.sharedFiles);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Folder");
                }
            }
            if (model.sharedFiles.IspublicORPrivate == "usergroup")
            {

                var users = _db.ApplicationUsers.Where(u => /*u.DepId == model.DepartmetId */  u.Id != claim.Value).ToList();
                if (users == null)
                {
                    TempData["error"] = "NO User is Assigned in this Department";
                    return RedirectToAction("Index", "Folder");
                }
                List<FileShared> Files = new List<FileShared>();
                foreach (var user in users)
                {
                    Files.Add(new FileShared
                    {
                        ReceiverId = user.Id,
                        SenderId = claim.Value,
                        DocumentId = model.produtId,
                        FileSentAt = DateTime.Now,
                        Isdowloadble = IsDwloadable,
                        IsUpdatable = IsUpdate,
                        IsSharedable = IsSharedable,

                    });
                }

                _db.FileShares.AddRange(Files);
                _db.SaveChanges();
                TempData[SD.succes] = "You Shared the file sucessfuly";
                return RedirectToAction("Index", "Folder");

            }
            return View(model);
        }

        List<Folder> list = new List<Folder>();
        private List<Folder> GetSubFolder(Folder req)
        {


            if (req.ParentFolderID != 0)
            {
                list.Add(req);
                Folder cat = _db.Folders.FirstOrDefault(x => x.FolderId == req.ParentFolderID);

                if (cat.ParentFolderID != 0)
                {
                    GetSubFolder(cat);
                }
            }
            return list;
        }
    }
}
