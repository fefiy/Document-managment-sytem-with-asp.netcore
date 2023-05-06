using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class shareFolder
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Sender")]
        [ValidateNever]
        public string SenderId { get; set; }

        [ForeignKey("Receiver")]

        [ValidateNever]

        public string ReceiverId { get; set; }
        public int FolderId { get; set; }
        [ForeignKey("FolderId")]
        [ValidateNever]

        public Folder Folder { get; set; }


        public ApplicationUser Sender { get; set; }
        [ValidateNever]

        public ApplicationUser Receiver { get; set; }

    

    }
}
