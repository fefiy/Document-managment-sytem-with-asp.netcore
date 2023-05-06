using System.ComponentModel.DataAnnotations;

namespace AFRICOMDMSWEB.Models
{
    public class AttendanceActions
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
