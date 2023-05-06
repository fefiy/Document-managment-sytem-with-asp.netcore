using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class ProjectViewModel
    {
        public Project project { get; set; }

        public TeamLeaders teamLeaders { get; set; }

        public BackEndDeveloperTeam backEndDeveloperTeam { get; set; }

        public FrontEndDeveloperTeam frontEndDeveloperTeam { get; set; }

        public UIDesignTeam UIDesignTeam { get; set; }

        public SolutionEnginneerTeam solutionEnginneerTeam { get; set; }
        public softwareArctectureTeam softwareArctectureTeam { get; set; }
        public TesterTeam testerTeam { get; set; }

        public Team team { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> projectLeaders { get; set; }
        [ValidateNever]

        public IEnumerable<SelectListItem> BackEndDevelopers { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> FrontEndDevelopers { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> sofwareArctecture { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> solutionEngineer { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> testers { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> UiDesigners { get; set; }
        [ValidateNever]

        public IEnumerable<SelectListItem> ProjectState { get; set; }
        [ValidateNever]

        public string CreateTeam { get; set; }
        public bool isLeaderSelected { get; set; }
        public bool isBackDevAssined { get; set; }
        public bool isFrontEndAssined { get; set; }
        public bool isSolutionEngAssined { get; set; }
        public bool isSoftwareArcAssined { get; set; }
        public bool isTesterAssined { get; set; }
        public bool isUIDesignerAssigned { get; set; }
    }
}
