using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Document
    {

        [Key]
        public int DocumentId { get; set; }

        public string DocumentTitle { get; set; }
        [ValidateNever]

        public string DocumentContentName { get; set; }
        [ValidateNever]

        public string DocumentContentType { get; set; }

        [ValidateNever]
        public string DocumentUrl { get; set; }

        [ValidateNever]
        public int FolderId { get; set; }

        [ForeignKey("FolderId")]
        [ValidateNever]
        public Folder Folder { get; set; }

        public float Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Description { get; set; }

        public string Tag { get; set; }

        public float Size { get; set; }

        [ValidateNever]
        [ForeignKey("UserCreater")]
       
        public string CreaterId { get; set; }

        [ForeignKey("userUpdater")]

        [ValidateNever]

        public string UpdaterId { get; set; }

        [ValidateNever]

        public ApplicationUser UserCreater { get; set; }
        [ValidateNever]

        public ApplicationUser userUpdater { get; set; }

    }
}
