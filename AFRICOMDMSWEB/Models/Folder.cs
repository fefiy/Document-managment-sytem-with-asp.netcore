using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Folder
    {
        [Key]
        public int FolderId { get; set; }
       
        public string FolderName { get; set; }
        [ValidateNever]
        public string path { get; set; }

        public int ParentFolderID { get; set; }
        public DateTime FolderCreatedAt { get; set; }
      
    }
}
