using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class RequestFile
    {
        [Key]
        [ValidateNever]
        public int RequestFileId { get; set; }

        [ValidateNever]
        public string RequestFileName { get; set; }
        [ValidateNever]

        public byte[] RequestFileContent { get; set; }
        [ValidateNever]

        public string RequestFileContentType { get; set; }

        [ValidateNever]
        public string RequestFileUrl { get; set; }
        [ValidateNever]

        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        [ValidateNever]

        public Request Request { get; set; }
    }
}
