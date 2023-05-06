using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class SolutionEnginneerTeam
    {
        public int Id { get; set; }
        [ValidateNever]

        public int  ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]

        public Project Project { get; set; }
        public string SolutionEnginneerId { get; set; }
        [ValidateNever]

        [ForeignKey("SolutionEnginneerId")]
        public ApplicationUser ApplicationUser { get; set;}

        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        [ValidateNever]
        public Team Team { get; set; }
        
    }
}
