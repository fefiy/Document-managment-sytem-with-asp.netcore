using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class RequestAction
    {
        [Key]
        public int Id { get; set; } 
        public string MessageForRequester { get; set; }
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
        [ValidateNever]

        public int CurrentStateId { get; set; }
       
        [ValidateNever]

        public int ActionId { get; set; }
        [ValidateNever]

        [ForeignKey("ActionId")]
        public Actions Actions { get; set; }
        [ValidateNever]

        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        [ValidateNever]

        public Request Request { get; set; }
        [ValidateNever]
        public DateTime ActionTakeDate { get; set; }
    }
}
