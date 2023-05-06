using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class ProjectFiles
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Filepath { get; set; }
        public byte[] Filecontent { get; set; }
        public string FileName { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]

        public Project Project { get; set; }
    }
}
