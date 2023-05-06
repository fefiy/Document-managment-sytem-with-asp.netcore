

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        [ValidateNever]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]
        public Project Project { get; set; }
        [Display(Name ="Team Name")]
        public string Name { get; set; }
    }
}
