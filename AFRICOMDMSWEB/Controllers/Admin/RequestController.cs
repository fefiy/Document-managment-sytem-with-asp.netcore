
using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using AFRICOMDMSWEB.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System.Configuration;
using System.Security.Claims;

namespace AFRICOMDMSWEB.Controllers.Admin
{
    public class RequestController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _conxt;

        private readonly AppDbContext _db;
        public RequestController(AppDbContext db, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor conxt)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _conxt = conxt;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var model = new RequestViewModel()
            {
                RquestSsended = _db.Requests.Where(a => a.RequesterId == claim.Value).Include(a => a.ActionTaker).ToList().OrderByDescending(a=> a.RedId),
                Rquestrecieved = _db.Requests.Where(x => x.ActionTakerId == claim.Value).Include(a => a.Requester).ToList().OrderByDescending(a => a.RedId),
                RequestsIRecivedFromPass = _db.Transitions.Where(a=> a.RequesterId == claim.Value).Include(a=> a.ActionTaker).OrderByDescending(a=> a.Id),
                RecivedRequiestPass = _db.Transitions.Where(a=> a.ActionTakerId== claim.Value).Include(a=> a.Requester).OrderByDescending(a => a.Id),
            };
            
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRequest()
        {
            var model = new AddReqFormViewModal()
            {
                RequiestForms = _db.RequiestForms.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRequest(AddReqFormViewModal model, IFormFile FormFile)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleName = _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == claim.Value).RoleId).Name;
            if (ModelState.IsValid)
            {
                string sendtoId = "";
                if(userRoleName== "BackEndDeveloper" || userRoleName == "FrontEndDeveloper" ||  userRoleName == "Solution Engineer" || userRoleName == "Software Arctecture" || userRoleName == "testerUser")
                {
                    // send to head of software enginnering
                    var roleId = _db.Roles.FirstOrDefault(r => r.Name == "Head of software Developer").Id;
                    sendtoId = _db.UserRoles.FirstOrDefault(u => u.RoleId == roleId).UserId;

                }
                else if(userRoleName == "Transport" || userRoleName == "HRUser" || userRoleName == "Security")
                {
                    //send to head of Hr head
                    var roleId = _db.Roles.FirstOrDefault(r => r.Name == "Head Of HR").Id;
                    sendtoId = _db.UserRoles.FirstOrDefault(u => u.RoleId == roleId).UserId;
                }
                else if (userRoleName == "UI/UX DesignerHead" || userRoleName == "Head of software Developer" || userRoleName == "Head Of HR" || userRoleName == "FinanceHead" || userRoleName == "ProjectManager" || userRoleName == "testerHead" || userRoleName== "TeamLeaader" || userRoleName == "Admin")
                {
                    //send to head of vicePresidant
                    var roleId = _db.Roles.FirstOrDefault(r => r.Name == "Vice Presidant").Id;
                    sendtoId = _db.UserRoles.FirstOrDefault(u => u.RoleId == roleId).UserId;
                }
                else if (userRoleName == "testerUser")
                {
                    //send to head of head of tester
                    var roleId = _db.Roles.FirstOrDefault(r => r.Name == "testerHead").Id;
                    sendtoId = _db.UserRoles.FirstOrDefault(u => u.RoleId == roleId).UserId;
                }
                else if (userRoleName == "Vice Presidant")
                {
                    var roleId = _db.Roles.FirstOrDefault(r => r.Name == "CEO Of Company").Id;
                    sendtoId = _db.UserRoles.FirstOrDefault(u => u.RoleId == roleId).UserId;
                    //send to head of head of ceo
                }


                model.Requests.RequestDate = DateTime.Now;
                model.Requests.RequesterId = claim.Value;
                model.Requests.ParetRequirmentId = 0;
                model.Requests.ActionTakerId = sendtoId;
                _db.Requests.Add(model.Requests);
                _db.SaveChanges();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (FormFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"AFRICOMDMS\RequistFile");
                    var extension = Path.GetExtension(FormFile.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        FormFile.CopyTo(fileStreams);
                    }
                  //  model.RequestFiles.RequestFileUrl = @"\AFRICOMDMS\RequistFile\" + fileName + extension;


                using (var memoryStream = new MemoryStream())
                {
                    await FormFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 2097152)
                    {
                            var reqFile = new RequestFile()
                            {
                                RequestFileUrl = @"\AFRICOMDMS\RequistFile\" + fileName + extension,
                                RequestFileContent = memoryStream.ToArray(),
                                RequestFileContentType = FormFile.ContentType,
                                RequestFileName = FormFile.FileName,
                                RequestId = model.Requests.RedId
                            };
                        //model.RequestFiles.RequestFileContent = memoryStream.ToArray();
                        //model.RequestFiles.RequestFileContentType = FormFile.ContentType;
                        //model.RequestFiles.RequestFileName = FormFile.FileName;
                        //model.RequestFiles.RequestId = model.Requests.RedId;

                        _db.RequestFiles.Add(reqFile);
                        _db.SaveChanges();

                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }

                }

                model.RequiestForms = _db.RequiestForms.ToList();
                TempData[SD.succes] = "Request Created successfuly";
                return RedirectToAction(nameof(Index));
            }
            model.RequiestForms = _db.RequiestForms.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult RequestRecieved()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var model = new RequestViewModel()
            {
                Rquestrecieved = _db.Requests.Where(x => x.ActionTakerId == claim.Value).Include(a => a.Requester).ToList().OrderByDescending(a => a.RedId),
                RecivedRequiestPass = _db.Transitions.Where(a => a.ActionTakerId == claim.Value).Include(a => a.Requester).OrderByDescending(a => a.Id)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult TakeAction(int id)
        {
            var model = new RequestActionViewModel()
            {
               ActionsType = _db.Actions.Select(a => new SelectListItem
               {
                   Text = a.Name,
                   Value = a.id.ToString()
               }),
               RequestID = id,
               request = _db.Requests.FirstOrDefault(r=> r.RedId == id)

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TakeAction(RequestActionViewModel model, IFormFile FormFile)
        {

            // where did irecieve this 
            int stateid = 1;
            var orgreq = _db.Requests.FirstOrDefault(f => f.RedId == model.RequestID);
            var requistid = _db.Requests.FirstOrDefault(f => f.RedId == model.RequestID).RedId;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var requister = _db.Requests.FirstOrDefault(f => f.RedId == model.RequestID).RequesterId;
            var requisterFromTra = _db.Transitions.FirstOrDefault(f => f.RequestId == requistid && f.ActionTakerId == claim.Value);
           if(requisterFromTra != null)
            {
                requister = requisterFromTra.RequesterId;
            }
            var reeqActions = _db.RequestActions.Where(r => r.RequestId == model.RequestID);
            if(reeqActions.Count() > 0)
            {
                stateid = model.CurrentStateId;
            }
            
            string pass = "Pass to Another Person";
            var passToAnother = _db.Actions.FirstOrDefault(f => f.Name == pass).id;
            var ActionTaken = new RequestAction()
            {
                RequesterId = requister,
                RequestId = model.RequestID,
                CurrentStateId = stateid,
                ActionTakeDate = DateTime.Now,
                ActionTakerId = claim.Value,
                MessageForRequester = model.requestAction.MessageForRequester,
                ActionId = model.requestAction.ActionId
            };

          
            if(model.requestAction.ActionId == passToAnother)
            {
                // create transition table
                string sendtoId = "";
                var userRoleName = _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == claim.Value).RoleId).Name;
                if(userRoleName == "UI/UX DesignerHead" || userRoleName == "Head of software Developer" || userRoleName == "Head Of HR" || userRoleName == "FinanceHead" || userRoleName == "ProjectManager" || userRoleName == "testerHead" || userRoleName == "TeamLeaader" || userRoleName == "Admin")
                {
                    //send to head of vicePresidant
                    var roleId = _db.Roles.FirstOrDefault(r => r.Name == "Vice Presidant").Id;
                    sendtoId = _db.UserRoles.FirstOrDefault(u => u.RoleId == roleId).UserId;
                }
                else
                {
                    var roleId = _db.Roles.FirstOrDefault(r => r.Name == "CEO Of Company").Id;
                    sendtoId = _db.UserRoles.FirstOrDefault(u => u.RoleId == roleId).UserId;
                    //send to head of head of ceo
                }


                var transition = new Transition()
                {
                    RequestId = model.RequestID,
                    RequesterId = claim.Value,
                    ActionTakerId =sendtoId,
                };
                _db.Transitions.Add(transition);


                if(model.writeComent == true)
                {
                    model.reqComment.SenderId = claim.Value;
                    model.reqComment.ReceiverId = sendtoId;
                    model.reqComment.RequestId = model.RequestID;
                    _db.ReqComments.Add(model.reqComment);
                }
            }
            else
            {
                // req answerFile is Attached
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (model.isfileAttach == true)
                {
                    if (FormFile != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"AFRICOMDMS\RequistFile");
                        var extension = Path.GetExtension(FormFile.FileName);
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            FormFile.CopyTo(fileStreams);
                        }

                      //  model.ReqAnswerFiles.RequestAnsFileUrl = @"\AFRICOMDMS\RequistFile\" + fileName + extension;


                        using (var memoryStream = new MemoryStream())
                        {
                            await FormFile.CopyToAsync(memoryStream);

                            if (memoryStream.Length < 2097152)
                            {
                                var file = new ReqAnswerFiles()
                                {
                                    RequestAnsFileUrl = @"\AFRICOMDMS\RequistFile\" + fileName + extension,
                                    RequestAnsFileContent = memoryStream.ToArray(),
                                    RequestAnsFileContentType = FormFile.ContentType,
                                    RequestAnsFileName = FormFile.FileName,
                                    RequestId = model.RequestID,
                                    ActionId = model.requestAction.ActionId,
                                    ActionTakerId = claim.Value,
                                    RequesterId = requister
                                };

                                _db.ReqAnswerFiles.Add(file);
                                _db.SaveChanges();

                            }
                            else
                            {
                                ModelState.AddModelError("File", "The file is too large.");
                            }
                        }


                    }
                    else
                    {
                        // formFileIsNone
                    }
                }
               

            }
            _db.RequestActions.Add(ActionTaken);
            _conxt.HttpContext.Session.SetInt32("idDetailss", model.RequestID);
            _db.SaveChanges();
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RequestActions(int id)
        {
            var Requist = _db.Requests.Where(r => r.RedId == id);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<RequestAction> model = _db.RequestActions.Where(f=> f.RequesterId == claim.Value && f.RequestId == id).Include(f=>f.ActionTaker);

            return View(model);
        }
        public IActionResult RequestAnswers(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var model = new RequestViewModel()
            {
                 SpecificRequestAnswers = _db.RequestActions.Where(f => f.RequesterId == claim.Value && f.RequestId == id).Include(f => f.ActionTaker),
                 requestAnswerFiles = _db.ReqAnswerFiles.Where(f => f.RequestId == id && f.RequesterId == claim.Value).Include(a => a.ActionTaker).Include(a => a.Actions)

            };
          //  IEnumerable<RequestAction> model = _db.RequestActions.Where(f => f.RequesterId == claim.Value && f.RequestId == id).Include(f => f.ActionTaker);

            return View(model);
        }

        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                var idDe = _conxt.HttpContext.Session.GetInt32("idDetailss");
                id = id.ToString() == null ? id : (int)idDe;

            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var model = new RequestViewModel()
            {
                Requests = _db.Requests.FirstOrDefault(r => r.RedId == id),
                RequestFiles = _db.RequestFiles.FirstOrDefault(r => r.RequestId == id),
                Comments = _db.ReqComments.Where(r => r.RequestId == id && r.ReceiverId == claim.Value).Include(s=> s.Sender),
                // = _db.Transitions.Where(a=> a.RequesterId == claim.Value && a.RequestId== id).Include(a=>a.ActionTaker),
                ActionUserTaken = _db.RequestActions.Where(a => a.ActionTakerId == claim.Value && a.RequestId == id).Include(a => a.Actions).Include(a => a.ActionTaker),
                RequestIpassToHigherLevel = _db.Transitions.Where(a => a.RequestId == id & a.RequesterId == claim.Value).Include(a=> a.ActionTaker),
                requestAnswerFiles = _db.ReqAnswerFiles.Where(f=> f.RequestId == id && f.RequesterId == claim.Value).Include(a=> a.ActionTaker).Include(a=> a.Actions)
                //ActionPassedForRequiest = _db.RequestActions.Where(a=> a.RequestId== id && claim.Value == a.RequesterId),
                //ActionPassedToAnotherForMe = _db.Transitions.Where(a=> a.ActionTakerId == claim.Value && a.RequestId == id).Include(a=> a.Requester)
            };
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var model = new RequestViewModel()
            {
                RequestFiles = _db.RequestFiles.FirstOrDefault(r => r.RequestId == id),
                Requests = _db.Requests.FirstOrDefault(a => a.RedId == id),
                RquestSsended = _db.Requests.Where(a => a.RedId == id).Include(a=> a.ActionTaker).ToList(),
            };
            return View(model);
        }


        
        public IActionResult AddReqFileForm()
        {
            return View();
        }

        
        public async Task<IActionResult> AddReqFileForm(RequiestForm model, IFormFile FormFile)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (FormFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"AFRICOMDMS\RequistFile");
                    var extension = Path.GetExtension(FormFile.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        FormFile.CopyTo(fileStreams);
                    }
                    model.FileUrl = @"\AFRICOMDMS\RequistFile\" + fileName + extension;

                }

                // adding the to value to the databse

                using (var memoryStream = new MemoryStream())
                {
                    await FormFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 2097152)
                    {
                        model.Filecontent = memoryStream.ToArray();
                        model.FileContentType = FormFile.ContentType;
                        model.FileName = FormFile.FileName;
                        _db.RequiestForms.Add(model);
                        _db.SaveChanges();
                        TempData[SD.succes] = "File add succesfuly";

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

       
        public IActionResult Download(int id)
        {
            byte[] bytes;
            string fileName, contentType;
            var dowDoc = _db.RequiestForms.FirstOrDefault(a => a.Id == id);
            if (dowDoc != null)
            {
                fileName = dowDoc.FileName;

                contentType = dowDoc.FileContentType;
                bytes =  dowDoc.Filecontent;
                return File(bytes, contentType, fileName);
            }
            return Ok("Can't find the Image");
        }

        public IActionResult DownloadAns(int id)
        {
            byte[] bytes;
            string fileName, contentType;
            var dowDoc = _db.ReqAnswerFiles.FirstOrDefault(a => a.RequestAnsFileId == id);
            if (dowDoc != null)
            {
                fileName = dowDoc.RequestAnsFileName;

                contentType = dowDoc.RequestAnsFileContentType;
                bytes = dowDoc.RequestAnsFileContent;
                return File(bytes, contentType, fileName);
            }
            return Ok("Can't find the Image");
        }

        [HttpGet]
        public IActionResult ReqProcess()
        {
            var model = new ReqPRocessViewModel()
            {
                selectRequestType = _db.ReqTypes.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.ReqTypeId.ToString()
                }),
                selectPearentRole = _db.Roles.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id

                })
            };
            return View(model);
        }

        [HttpPost]

        public IActionResult ReqProcess(ReqPRocessViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.ReqProcesss.Add(model.reqProcess);
                _db.SaveChanges();
            }
            model.selectRequestType = _db.ReqTypes.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.ReqTypeId.ToString()
            });
            model.selectPearentRole = _db.Roles.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id

            });
            return View(model);
        }



        public IActionResult DownloadReq(int id)
        {
            byte[] bytes;
            string fileName, contentType;
            var dowDoc = _db.RequestFiles.FirstOrDefault(a => a.RequestFileId == id);
            if (dowDoc != null)
            {
                fileName = dowDoc.RequestFileName;

                contentType = dowDoc.RequestFileContentType;
                bytes = dowDoc.RequestFileContent;
                return File(bytes, contentType, fileName);
            }
            return Ok("Can't find the Image");
        }

    }
}
