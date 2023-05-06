using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        [ValidateNever]

        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Clients { get; set; }
        public int IsAssignedTeamLeader { get; set; }
        public int IsAssignedBackend { get; set; }
        public int IsAssignedFrontEnd { get; set; }
        public int IsAssignedUIDesign { get; set; }
        public int IsAssignedTester { get; set; }

        public int IsAssirnedSolutionEngineer { get; set; }
        public int IsAssignedSoftwareArctecture { get; set; }
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        [ValidateNever]

        public ProjectState projectState { get; set; }
    }
}
