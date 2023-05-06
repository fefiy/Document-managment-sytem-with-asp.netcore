using System.ComponentModel.DataAnnotations;

namespace AFRICOMDMSWEB.Models
{
    public class ProjectState
    {
        [Key]
        public int Id { get; set; }

        public string StateName { get; set; }

        public int level { get; set; }
    }
}
