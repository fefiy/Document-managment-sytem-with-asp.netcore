using AFRICOMDMSWEB.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class DocumentVersionViewModel
    {
        [ValidateNever]
        public Document Documents { get; set; }
        [ValidateNever]
        public DocumentVersions DocumentVersions { get; set; }

        [ValidateNever]
        public int FileId { get; set; }

        [NotMapped]
        public int  fileeeID { get; set; }

        public IEnumerable<Document> documents { get; set; }
        public IEnumerable<DocumentVersions> documentsVersion { get; set; }
        [ValidateNever]
        public int size { get; set; }
    }
}
