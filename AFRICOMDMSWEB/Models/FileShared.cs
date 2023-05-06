using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class FileShared
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Sender")]
        [ValidateNever]
        public string SenderId { get; set; }

        [ForeignKey("Receiver")]

        [ValidateNever]

        public string ReceiverId { get; set; }
        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        [ValidateNever]

        public Document Document { get; set; }

        public DateTime FileSentAt { get; set; }
        [ValidateNever]

        public ApplicationUser Sender { get; set; }
        [ValidateNever]

        public ApplicationUser Receiver { get; set; }

        public int IsSharedable { get; set; }

        public int IsUpdatable { get; set; }

        public int Isdowloadble { get; set; }

        [NotMapped]
        public  string IspublicORPrivate { get; set; }
    }
}

//1=> application sender have many recieverId
//1=> a
//pplication reciever also have many senderId so betwwen reciever and sender  have many to many relarionship


//[ForeignKey("UserCreater")]
//[ValidateNever]
//public string CreaterId { get; set; }

//[ForeignKey("userUpdater")]

//[ValidateNever]

//public string UpdaterId { get; set; }

//[ValidateNever]

//public ApplicationUser UserCreater { get; set; }
//[ValidateNever]

//public ApplicationUser userUpdater { get; set; }