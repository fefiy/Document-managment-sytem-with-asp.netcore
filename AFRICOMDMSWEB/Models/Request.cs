using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Request
    {
        [Key]
        public int RedId { get; set; }
        [ValidateNever]
        public string ActionTakerId { get; set; }
        [ForeignKey("ActionTakerId")]

        [ValidateNever]
        public ApplicationUser ActionTaker { get; set; }
        [ValidateNever]
        public string RequesterId { get; set; }
        [ForeignKey("RequesterId")]
        [ValidateNever]
        public ApplicationUser Requester { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public DateTime RequestDate { get; set; }
        public int ParetRequirmentId {get; set;}
        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem>   ActionTakers { get; set; }

    }
}
