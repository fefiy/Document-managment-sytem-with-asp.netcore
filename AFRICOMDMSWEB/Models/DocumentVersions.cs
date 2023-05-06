using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class DocumentVersions
    {
        [Key]
        public int VersionsId { get; set; }
        [ValidateNever]

        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        [ValidateNever]
        public Document Document { get; set; }
        [ValidateNever]

        public string UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]

        public DateTime UpdatedAt { get; set; }
        [ValidateNever]
        public float Versions { get; set; }
        [ValidateNever]

        public string DocumentVersionUrl { get; set; }

        [ValidateNever]
        public float Size { get; set; }
        [NotMapped]
        public int FileId { get; set; }

    }
}
