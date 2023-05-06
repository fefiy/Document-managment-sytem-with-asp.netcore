using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using AFRICOMDMSWEB.Models.ViewModel;
using Dms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Claims;



namespace AFRICOMDMSWEB.Controllers.Admin
{
    [Authorize]
    public class FolderController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor conxt;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IServer _server;
        public static string pathafri = "";

        public FolderController(IServer server, IWebHostEnvironment webHostEnvironment, AppDbContext db, IHttpContextAccessor httpContext, UserManager<IdentityUser> userManager)
        {
            _server = server;
            conxt = httpContext;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            pathafri = "AFRICOMDMS";
           conxt.HttpContext.Session.SetString("path", "AFRICOMDMS");
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var folderRoot = _db.Folders.FirstOrDefault(f => f.path == "AFRICOMDMS");

            var doc = new DocumentViewModel()
            {
                path= pathafri,
                //path = conxt.HttpContext.Session.GetString("path"),
            //  folder = _db.Folders.Where(f => f.path != "AFRICOMDMS" && f.ParentFolderID == folderRoot.FolderId /*&& f.CreatedbyID == claim.Value*/).ToList(),
                folder_Users = _db.Folder_Users.Include(f=> f.Folder).Where(f=> f.CreatedbyID== claim.Value && f.Folder.ParentFolderID == folderRoot.FolderId), 
                documents = _db.Documents.Where(d => d.FolderId == folderRoot.FolderId && d.CreaterId == claim.Value).ToList(),
                FileRecived = _db.FileShares.Include(d => d.Document).Where(d => d.ReceiverId == claim.Value && d.Document.FolderId == folderRoot.FolderId),
                FolderRecived = _db.ShareFolders.Include(f => f.Folder).Where(f => f.ReceiverId == claim.Value && f.Folder.ParentFolderID == folderRoot.FolderId).ToList(),
            };

            return View(doc);
        }
        [HttpGet]
        public IActionResult CreateFolder()
        {
            DocumentViewModel doc = new DocumentViewModel()
            {
                path = pathafri
              //  path = conxt.HttpContext.Session.GetString("path"),
                // folder = _db.folders.Where(f=> f.FolderId != 1).ToList()
            };

            return View(doc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFolder(DocumentViewModel model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var users = _db.ApplicationUsers.Where(a => a.Id == claim.Value);
            //   var path = conxt.HttpContext.Session.GetString("path");
            var path = pathafri;
            var pathThefileStored = "AFRICOMDMS";
          //  var UserFolser =  _db.Folders.Where(a => a.CreatedbyID == claim.Value).ToList();
            var directory = Path.Combine(_webHostEnvironment.WebRootPath, pathThefileStored, model.Folders.FolderName);
            var folderRoot = _db.Folders.FirstOrDefault(f => f.path == "AFRICOMDMS");
            var userFolder = _db.Folder_Users.Include(f => f.Folder).Where(f => f.CreatedbyID == claim.Value);

            model.Folders.path = $"{path}\\{model.Folders.FolderName}";
            model.Folders.FolderName = model.Folders.FolderName;
          //  model.Folders.CreatedbyID = claim.Value;
            model.Folders.FolderCreatedAt = DateTime.Now;

            model.Folders.ParentFolderID = _db.Folders.FirstOrDefault(f => f.path == path).FolderId;

            //}


            foreach (var fol in userFolder)
            {
                if (fol.Folder.path == model.Folders.path)
                {
                    TempData[SD.error] = "Folder Already Exist";

                    if (model.Folders.ParentFolderID == folderRoot.FolderId)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction("folderst");
                    }
                }
            }
           
            _db.Folders.Add(model.Folders);
            _db.SaveChanges();

            var fol_user = new Folder_User()
            {
                CreatedbyID = claim.Value,
                FolderId = model.Folders.FolderId,
            };
            _db.Folder_Users.Add(fol_user);
            _db.SaveChanges();
            Directory.CreateDirectory(directory);



            if (model.Folders.path == "AFRICOMDMS")
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("folderst");
            }
        }


        [HttpGet]
        public IActionResult folderst(int id)
        {

            //var Curpath = conxt.HttpContext.Session.GetString("path");
            var Curpath = pathafri;
            if (id == 0)
            {
                id = _db.Folders.FirstOrDefault(f => f.path == Curpath).FolderId;
            }

            var FolNow = _db.Folders.FirstOrDefault(f => f.FolderId == id);
            string path = FolNow.path;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            DocumentViewModel doc = new DocumentViewModel()
            {
                path = path,
                documents = _db.Documents.Where(doc => doc.FolderId == id && doc.CreaterId == claim.Value),
               // folder = _db.Folders.Where(f => f.ParentFolderID == id && f.CreatedbyID == claim.Value),
        //        folderfromDms = _db.Folders.Where(f => f.FolderId != 1 && f.ParentFolderID == 1 && f.CreatedbyID == claim.Value).ToList(),
                pathFolders = GetSubFolders(FolNow).ToList().OrderBy(f => f.ParentFolderID),
                FolderRecived = _db.ShareFolders.Include(f => f.Folder).Where(f => f.ReceiverId == claim.Value && f.Folder.ParentFolderID == id).ToList(),
                FileRecived = _db.FileShares.Include(f => f.Document).Where(f => f.ReceiverId == claim.Value && f.Document.FolderId == id),
                folder_Users = _db.Folder_Users.Include(f => f.Folder).Where(f => f.CreatedbyID == claim.Value && f.Folder.ParentFolderID == id),

                //  pathhh = string.Join(", ", addresses)
            };
            //conxt.HttpContext.Session.Remove("path");
            //conxt.HttpContext.Session.SetString("path", path);
            pathafri = path;
            return View(doc);
        }

        public IActionResult CreateFile()
        {

            DocumentViewModel doc = new DocumentViewModel()
            {
                path = pathafri
                /*conxt.HttpContext.Session.GetString("path")*/
            };
            return View(doc);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile(DocumentViewModel document, IFormFile FormFile)
        {
            var pathThefileStored = "AFRICOMDMS";
            //var pathcurrent = conxt.HttpContext.Session.GetString("path");
            var pathcurrent = pathafri;
            var count = _db.Documents.Count();

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (FormFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, pathThefileStored);
                    var extension = Path.GetExtension(FormFile.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        FormFile.CopyTo(fileStreams);
                    }
                    document.Documents.DocumentUrl = $"{pathThefileStored}\\" + fileName + extension;
                }


                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //save image to database.
                using (var memoryStream = new MemoryStream())
                {
                    await FormFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        //Convert the View 
                        document.Documents.CreatedAt = DateTime.Now;
                        document.Documents.DocumentContentName = FormFile.FileName;
                        //   document.Documents.DocumentContent = memoryStream.ToArray();
                        document.Documents.FolderId = _db.Folders.FirstOrDefault(f => f.path == pathcurrent).FolderId;
                        document.Documents.DocumentContentType = FormFile.ContentType;
                        document.Documents.CreaterId = claim.Value;
                        document.Documents.UpdaterId = claim.Value;
                        document.Documents.Version = count + 1;
                        document.Documents.Size = (float)(memoryStream.Length / 1000);
                        _db.Documents.Add(document.Documents);

                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
                var pathnow = pathafri;
                    /*conxt.HttpContext.Session.GetString("path");*/
                if (pathnow == "AFRICOMDMS")
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return RedirectToAction(nameof(folderst));
                }
            }
            document.path = pathafri /*conxt.HttpContext.Session.GetString("path")*/;
            return View(document);
        }


        //public IActionResult DownloadImage(int id)
        //{
        //    byte[] bytes;
        //    string fileName, contentType;
        //    var item = _db.Documents.FirstOrDefault(c => c.DocumentId == id);

        //    if (item != null)
        //    {
        //        fileName = item.DocumentContentName;
        //        contentType = item.DocumentContentType;
        //      //  bytes = item.DocumentContent;
        //        return File(bytes, contentType, fileName);
        //    }
        //    return Ok("Can't find the Image");
        //}
        public IActionResult DownloadFromPath(int id)
        {
            byte[] bytes;
            string fileName, contentType;
            var item = _db.Documents.FirstOrDefault(c => c.DocumentId == id);


            if (item != null)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, item.DocumentUrl);
                fileName = item.DocumentContentName;
                contentType = item.DocumentContentType;
                bytes = System.IO.File.ReadAllBytes(path);
                return File(bytes, contentType, fileName);
            }
            return Ok("Can't find the Image");
        }
        public IActionResult DownloadDocV(int id)
        {
            byte[] bytes;
            string fileName, contentType;
            var item = _db.DocumentVersions.FirstOrDefault(c => c.VersionsId == id);
            var lastUpDoc = _db.Documents.FirstOrDefault(f => f.DocumentId == item.DocumentId);
            if (item != null)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, lastUpDoc.DocumentUrl);

                fileName = lastUpDoc.DocumentContentName;

                contentType = lastUpDoc.DocumentContentType;
                bytes = System.IO.File.ReadAllBytes(path);
                return File(bytes, contentType, fileName);
            }
            return Ok("Can't find the Image");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            //conxt.HttpContext.Session.SetInt32("deleteDocId", id);
            var delpro = _db.Documents.FirstOrDefault(u => u.DocumentId == id);
            var DocVersions = _db.DocumentVersions.Where(d => d.DocumentId == delpro.DocumentId);
            var sharedDoc = _db.FileShares.Where(d => d.DocumentId == id);
            if (delpro == null)
            {
                return NotFound();
            }
            if (DocVersions.Count() > 0)
            {
                return View("ConfirmDeletion");
            }
            var oldimage = Path.Combine(_webHostEnvironment.WebRootPath, delpro.DocumentUrl.TrimStart('\\'));
            var DocIsShared = _db.FileShares.Where(d => d.DocumentId == id);
            if (DocIsShared.Count() > 0)
            {
                TempData[SD.error] = "You can't Delete this Document It is Shared";
            }
            else
            {
                if (DocIsShared.Count() < 1)
                {
                    if (System.IO.File.Exists(oldimage))
                    {
                        System.IO.File.Delete(oldimage);
                    }

                    _db.Documents.Remove(delpro);
                    _db.SaveChanges();
                    TempData[SD.error] = "the file Deleted succesfully";
                }
                else
                {

                }
            }
            var pathnow = pathafri;
            /*conxt.HttpContext.Session.GetString("path");*/
            if (pathnow == "AFRICOMDMS")
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(folderst));
            }
        }

        public async Task<IActionResult> DeleteVAlso()
        {
            var id = conxt.HttpContext.Session.GetInt32("deleteDocId");
            var delpro = _db.Documents.FirstOrDefault(u => u.DocumentId == id);
            var DocVersions = _db.DocumentVersions.Where(d => d.DocumentId == delpro.DocumentId).ToList();
            if (delpro == null)
            {
                return NotFound();
            }
            if (DocVersions.Count() > 0)
            {
                _db.DocumentVersions.RemoveRange(DocVersions);
                //   _db.SaveChangesAsync();

                foreach (var doc in DocVersions)
                {

                    var oldimage = Path.Combine(_webHostEnvironment.WebRootPath, doc.DocumentVersionUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldimage))
                    {
                        System.IO.File.Delete(oldimage);
                    }

                }
            }
            var oldimag = Path.Combine(_webHostEnvironment.WebRootPath, delpro.DocumentUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldimag))
            {
                System.IO.File.Delete(oldimag);
            }

            _db.Documents.Remove(delpro);
            _db.SaveChanges();
            TempData[SD.error] = "the file Deleted succesfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult UpdateFileVersion(int id)
        {

            var model = new DocumentVersions()
            {
                FileId = id,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFileVersion(DocumentVersions model, IFormFile FormFile)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var DocFromDb = _db.Documents.FirstOrDefault(f => f.DocumentId == model.FileId);
            if (DocFromDb == null)
            {
                return NotFound();
            }
            float existSize = DocFromDb.Size;
            float exstVersion = DocFromDb.Version;
            //    byte[] existFileContent = DocFromDb.DocumentContent;
            string DocUrlExist = DocFromDb.DocumentUrl;
            var existCreaator = DocFromDb.UpdaterId;

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (FormFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"AFRICOMDMS\DocumentVersioControlsss");
                    var extension = Path.GetExtension(FormFile.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        FormFile.CopyTo(fileStreams);
                    }
                    DocFromDb.DocumentUrl = @"AFRICOMDMS\DocumentVersioControlsss\" + fileName + extension;

                }

                // adding the to value to the databse

                using (var memoryStream = new MemoryStream())
                {
                    await FormFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 2097152)
                    {
                        // update document
                        //   DocFromDb.DocumentContent = memoryStream.ToArray();
                        DocFromDb.Version = exstVersion + 0.1F;
                        DocFromDb.Size = (float)(memoryStream.Length / 1000);
                        DocFromDb.UpdaterId = claim.Value;
                        _db.Documents.Update(DocFromDb);

                        await _db.SaveChangesAsync();

                        // add the oldest version to the table

                        //      model.DocumentVersionsContent = existFileContent;
                        model.DocumentVersionUrl = DocUrlExist;
                        model.DocumentId = model.FileId;
                        model.UpdatedById = existCreaator;
                        model.UpdatedAt = DateTime.Now;
                        model.Versions = exstVersion;
                        model.Size = existSize;
                        _db.DocumentVersions.Add(model);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");

                    }
                }
                return RedirectToAction(nameof(Index));


            }
            return View(model);

        }



        [HttpGet]
        public IActionResult GetAllVersionDocument()
        {
            var id = conxt.HttpContext.Session.GetInt32("idD");
            IEnumerable<DocumentVersions> docVersions = _db.DocumentVersions.Where(d => d.DocumentId == id).Include(a => a.ApplicationUser).ToList();

            return View(docVersions);
        }


        public IActionResult Detail(int id)
        {
            conxt.HttpContext.Session.SetInt32("idD", id);
            var idDe = conxt.HttpContext.Session.GetInt32("idD");
            //id = id ??= idDe;
            id = id.ToString() == null ? id : (int)idDe;
            // var path = _db.Documents.FirstOrDefault(d => d.DocumentId == id).DocumentUrl;

            var model = new DocumentVersionViewModel()
            {
                documentsVersion = _db.DocumentVersions.Where(d => d.DocumentId == id).Include(a => a.ApplicationUser).Include(d => d.Document),
                Documents = _db.Documents.FirstOrDefault(d => d.DocumentId == id),
                //size = (int)new FileInfo(path).Length,
            };
            return View(model);
        }

       // public static int idDetail = 0;

        //[HttpGet]
        //public IActionResult Edit()
        //{
        //    var id = conxt.HttpContext.Session.GetInt32("idD");
        //    var docfromdB = _db.Documents.FirstOrDefault(d => d.DocumentId == id);
        //    return View(docfromdB);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Document model, IFormFile FormFile)
        //{

        //    var id = conxt.HttpContext.Session.GetInt32("idD");
        //    var docfromdB = _db.Documents.FirstOrDefault(d => d.DocumentId == id);
        //    var pathcurrent = _db.Folders.FirstOrDefault(f => f.FolderId == docfromdB.FolderId).path;

        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        if (FormFile != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString();
        //            var uploads = Path.Combine(wwwRootPath, pathcurrent);
        //            var extension = Path.GetExtension(FormFile.FileName);

        //            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
        //            {
        //                FormFile.CopyTo(fileStreams);
        //            }

        //            model.DocumentUrl = $"{pathcurrent}\\" + fileName + extension;
        //        }


        //        var claimsIdentity = (ClaimsIdentity)User.Identity;
        //        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        //        //save image to database.
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await FormFile.CopyToAsync(memoryStream);

        //            // Upload the file if less than 2 MB
        //            if (memoryStream.Length < 2097152)
        //            {
        //                //Convert the View 
        //                //  document.Documents.CreatedAt = DateTime.Now;
        //                model.DocumentContentName = FormFile.FileName;
        //                //        model.DocumentContent = memoryStream.ToArray();

        //                // model.FolderId = _db.folders.FirstOrDefault(f => f.path == pathcurrent).FolderId;
        //                //  model.DocumentContentType = FormFile.ContentType;
        //                // model.CreatedbyID = claim.Value;
        //                // model.Version = count + 1;
        //                model.Size = (float)(memoryStream.Length / 1000);

        //                /// upadate db
        //                /// 
        //                docfromdB.Description = model.Description;
        //                docfromdB.DocumentTitle = model.DocumentTitle;
        //                docfromdB.DocumentUrl = model.DocumentUrl;
        //                docfromdB.Tag = model.Tag;
        //                docfromdB.Size = model.Size;
        //                docfromdB.DocumentContentName = model.DocumentContentName;
        //                //     docfromdB.DocumentContent = model.DocumentContent;
        //                _db.Documents.Update(docfromdB);
        //                await _db.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("File", "The file is too large.");
        //            }
        //        }
        //        var pathnow = conxt.HttpContext.Session.GetString("path");
        //        if (pathnow == "AFRICOMDMS")
        //        {
        //            return RedirectToAction(nameof(Index));

        //        }
        //        else
        //        {
        //            return RedirectToAction(nameof(folderst));
        //        }
        //    }
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> DeleteFolders(int id)
        {
            var pathnow = pathafri;
                /*conxt.HttpContext.Session.GetString("path");*/

            var FoldeDel = _db.Folders.FirstOrDefault(f => f.FolderId == id);
            if (FoldeDel == null)
            {
                return NotFound();
            }
            var subFolder = _db.Folders.Where(f => f.ParentFolderID == id);
            var DocInFolder = _db.Documents.Where(d => d.FolderId == id);
            var userzfol = _db.Folder_Users.Where(d => d.FolderId == id);
            if (DocInFolder.Count() > 0 || subFolder.Count() > 0)
            {
                TempData[SD.error] = "The Folder you want to remove has documents in it";
            }
            else
            {
                _db.Folder_Users.RemoveRange(userzfol);
                _db.SaveChanges();
                _db.Folders.Remove(FoldeDel);
                _db.SaveChanges();
            }
            if (pathnow == "AFRICOMDMS")
            {
                return RedirectToAction(nameof(Index));

            }
            else
            {
                return RedirectToAction(nameof(folderst));
            }
        }

        List<Folder> list = new List<Folder>();
        private List<Folder> GetSubFolders(Folder req)
        {
            if (req.ParentFolderID != 0)
            {
                list.Add(req);

                Folder cat = _db.Folders.FirstOrDefault(x => x.FolderId == req.ParentFolderID);

                if (cat.ParentFolderID != 0)
                {
                    GetSubFolders(cat);
                }

            }
            return list;
        }

    }
}




