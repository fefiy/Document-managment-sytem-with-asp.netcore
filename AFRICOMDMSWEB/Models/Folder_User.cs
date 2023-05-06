using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Folder_User
    {
        public int Id { get; set; }

        public int FolderId { get; set; }

        [ForeignKey("FolderId")]

        public Folder Folder { get; set; }
        public string CreatedbyID { get; set; }
        [ForeignKey("CreatedbyID ")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
