using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace AFRICOMDMSWEB.Models
{
    public class ReviewDoc
    {
        [Key]
        public int ReviewId { get; set; }

        public string message { get; set; }

        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        [ValidateNever]
        public Document Document { get; set; }

        public string ReviewerId { get; set; }
        [ForeignKey("ReviewerId")]
        [ValidateNever]
        public ApplicationUser Reviewer { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
