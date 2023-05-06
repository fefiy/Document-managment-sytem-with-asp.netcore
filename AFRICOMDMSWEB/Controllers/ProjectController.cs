
using AFRICOMDMSWEB.Data;
using AFRICOMDMSWEB.Models;
using AFRICOMDMSWEB.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Dms.Controllers
{

   
    [Authorize(Roles = "ProjectManager")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }
        
        public IActionResult Index()
        {
           
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            var leaderId = _db.Roles.FirstOrDefault(a => a.Name == "TeamLeaader").Id;
            var UiDeId = _db.Roles.FirstOrDefault(a => a.Name == "UIDesignUser").Id;
            var TesterId = _db.Roles.FirstOrDefault(a => a.Name == "testerUser").Id;
            var solutionEnginerId = _db.Roles.FirstOrDefault(a => a.Name == "Solution Engineer").Id;
            var FrontEndDeveloperId = _db.Roles.FirstOrDefault(a => a.Name == "FrontEndDeveloper").Id;
            var BackEndDeveloperId = _db.Roles.FirstOrDefault(a => a.Name == "BackEndDeveloper").Id;
            var softwareArcId = _db.Roles.FirstOrDefault(a => a.Name == "Software Arctecture").Id;
            List<string> useridLeader = _db.UserRoles.Where(a => a.RoleId == leaderId).Select(b => b.UserId).Distinct().ToList();
            List<string> useridUIDesign = _db.UserRoles.Where(a => a.RoleId == UiDeId).Select(b => b.UserId).Distinct().ToList();
            List<string> useridTester = _db.UserRoles.Where(a => a.RoleId == TesterId).Select(b => b.UserId).Distinct().ToList();
            List<string> useridFrontEnd = _db.UserRoles.Where(a => a.RoleId == FrontEndDeveloperId).Select(b => b.UserId).Distinct().ToList();
            List<string> useridSolutionEngineer = _db.UserRoles.Where(a => a.RoleId == solutionEnginerId).Select(b => b.UserId).Distinct().ToList();
            List<string> useridBackend = _db.UserRoles.Where(a => a.RoleId == BackEndDeveloperId).Select(b => b.UserId).Distinct().ToList();
            List<string> useridSoftwareArc = _db.UserRoles.Where(a => a.RoleId == softwareArcId).Select(b => b.UserId).Distinct().ToList();
            List<string> alreadyAssignedteamLeaderId = _db.TeamLeaders.Select(b => b.LeaderId).Distinct().ToList();

            List<ApplicationUser> listLeaderUsers = _db.ApplicationUsers.Where(a => useridLeader.Any(c => c == a.Id) && alreadyAssignedteamLeaderId.All(c=> c != a.Id)).ToList();
            List<ApplicationUser> listTesterUsers = _db.ApplicationUsers.Where(a => useridTester.Any(c => c == a.Id)).ToList();
            List<ApplicationUser> listUIDesignUsers = _db.ApplicationUsers.Where(a => useridUIDesign.Any(c => c == a.Id)).ToList();
            List<ApplicationUser> listBackendUsers = _db.ApplicationUsers.Where(a => useridBackend.Any(c => c == a.Id)).ToList();
            List<ApplicationUser> listFrontEndUsers = _db.ApplicationUsers.Where(a => useridFrontEnd.Any(c => c == a.Id)).ToList();
            List<ApplicationUser> listSolutionEngUsers = _db.ApplicationUsers.Where(a => useridSolutionEngineer.Any(c => c == a.Id)).ToList();
            List<ApplicationUser> listSoftwareArcUsers = _db.ApplicationUsers.Where(a => useridSoftwareArc.Any(c => c == a.Id)).ToList();


            var model = new ProjectViewModel()
            {
                ProjectState = _db.ProjectStates.Select(p => new SelectListItem
                {
                    Text = p.StateName,
                    Value = p.Id.ToString()
                }),
                projectLeaders = listLeaderUsers.Select(x => new SelectListItem
                {
                    Text = x.Name + "->"+_db.Roles.FirstOrDefault(r=> r.Id == _db.UserRoles.FirstOrDefault(u=> u.UserId==x.Id).RoleId).Name,
                    Value = x.Id
                }),
                UiDesigners = listUIDesignUsers.Select(x => new SelectListItem
                {
                    Text = x.Name + "->" + _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == x.Id).RoleId).Name,
                    Value = x.Id
                }),
                testers = listTesterUsers.Select(x => new SelectListItem
                {
                    Text = x.Name + "->" + _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == x.Id).RoleId).Name,
                    Value = x.Id
                }),
               BackEndDevelopers = listBackendUsers.Select(x => new SelectListItem
                {
                    Text = x.Name + "->" + _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == x.Id).RoleId).Name,
                    Value = x.Id
                }),
                FrontEndDevelopers= listFrontEndUsers.Select(x => new SelectListItem
                {
                    Text = x.Name + "->" + _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == x.Id).RoleId).Name,
                    Value = x.Id
                }),

                sofwareArctecture= listSoftwareArcUsers.Select(x => new SelectListItem
                {
                    Text = x.Name + "->" + _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == x.Id).RoleId).Name,
                    Value = x.Id
                }),

                solutionEngineer = listSolutionEngUsers.Select(x => new SelectListItem
                {
                    Text = x.Name + "->" + _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == x.Id).RoleId).Name,
                    Value = x.Id
                }),
            };
           
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateProject(ProjectViewModel model)
        {
            if(model.CreateTeam== "later")
            {
                model.project.CreatedDate = DateTime.Now;
                _db.Projects.Add(model.project);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                model.project.CreatedDate = DateTime.Now;
                _db.Projects.Add(model.project);
                _db.SaveChanges();
                if(model.CreateTeam == "now")
                {
                    model.team.ProjectId = model.project.ProjectId;
                    _db.Teams.Add(model.team);
                    _db.SaveChanges();
                    // team leaders
                    if (model.isLeaderSelected == true)
                    {
                        model.teamLeaders.ProjectId = model.project.ProjectId;
                        model.teamLeaders.TeamId = model.team.TeamId;
                        model.project.IsAssignedTeamLeader = 1;
                        _db.TeamLeaders.Add(model.teamLeaders);
                        _db.SaveChanges();
                    }
                    // solution engineer
                    if (model.isSolutionEngAssined == true)
                    {
                        model.solutionEnginneerTeam.ProjectId = model.project.ProjectId;
                        model.solutionEnginneerTeam.TeamId = model.team.TeamId;
                        model.project.IsAssirnedSolutionEngineer = 1;
                        _db.SolutionEnginneerTeams.Add(model.solutionEnginneerTeam);
                        _db.SaveChanges();
                    }
                    // software Arctecture
                    if (model.isSoftwareArcAssined == true)
                    {
                        model.softwareArctectureTeam.ProjectId = model.project.ProjectId;
                        model.softwareArctectureTeam.TeamId = model.team.TeamId;
                        model.project.IsAssignedSoftwareArctecture = 1;
                        _db.SoftwareArctectureTeams.Add(model.softwareArctectureTeam);
                        _db.SaveChanges();
                    }
                    //ui designers
                    if (model.isUIDesignerAssigned == true)
                    {
                        model.UIDesignTeam.ProjectId = model.project.ProjectId;
                        model.UIDesignTeam.TeamId = model.team.TeamId;
                        model.project.IsAssignedUIDesign= 1;
                        _db.UIDesignTeams.Add(model.UIDesignTeam);
                        _db.SaveChanges();
                    }
                    //back end Developers
                    if (model.isBackDevAssined == true)
                    {
                        model.backEndDeveloperTeam.ProjectId = model.project.ProjectId;
                        model.backEndDeveloperTeam.TeamId = model.team.TeamId;
                        model.project.IsAssignedBackend = 1;
                        _db.BackEndDeveloperTeams.Add(model.backEndDeveloperTeam);
                        _db.SaveChanges();
                    }

                    //frontEnd Designers

                    if (model.isFrontEndAssined == true)
                    {
                        model.frontEndDeveloperTeam.ProjectId = model.project.ProjectId;
                        model.frontEndDeveloperTeam.TeamId = model.team.TeamId;
                        model.project.IsAssignedFrontEnd = 1;
                        _db.FrontEndDeveloperTeams.Add(model.frontEndDeveloperTeam);
                        _db.SaveChanges();
                    }


                    // testers
                    if (model.isTesterAssined == true)
                    {
                        model.testerTeam.ProjectId = model.project.ProjectId;
                        model.testerTeam.TeamId = model.team.TeamId;
                        model.project.IsAssignedTester = 1;
                        _db.TesterTeams.Add(model.testerTeam);
                        _db.SaveChanges();
                    }
                    return RedirectToAction("Index");

                }
               
                return RedirectToAction("Index");

            }
            return View(model);
        }


    }
}
