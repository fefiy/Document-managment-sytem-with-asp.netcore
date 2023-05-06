using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class ReqAnswerFiles
    {
        [Key]
        [ValidateNever]
        public int RequestAnsFileId { get; set; }

        
        [ValidateNever]
        public string RequestAnsFileName { get; set; }
        [ValidateNever]

        public byte[] RequestAnsFileContent { get; set; }
        [ValidateNever]

        public string RequestAnsFileContentType { get; set; }

        [ValidateNever]
        public string RequestAnsFileUrl { get; set; }
        [ValidateNever]

        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        [ValidateNever]

        public Request Request { get; set; }

        public int ActionId { get; set; }
        [ValidateNever]

        [ForeignKey("ActionId")]
        public Actions Actions { get; set; }

        public string ActionTakerId { get; set; }
        [ForeignKey("ActionTakerId")]

        [ValidateNever]
        public ApplicationUser ActionTaker { get; set; }
        [ValidateNever]
        public string RequesterId { get; set; }
        [ForeignKey("RequesterId")]
        [ValidateNever]
        public ApplicationUser Requester { get; set; }
    }
}
