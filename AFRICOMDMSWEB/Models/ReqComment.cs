using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class ReqComment
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        [ForeignKey("Sender")]
        [ValidateNever]
        public string SenderId { get; set; }

        [ForeignKey("Receiver")]

        [ValidateNever]

        public string ReceiverId { get; set; }

        [ValidateNever]

        public ApplicationUser Sender { get; set; }
        [ValidateNever]

        public ApplicationUser Receiver { get; set; }
        [ValidateNever]
        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        [ValidateNever]

        public Request Request { get; set; }
    }
}
