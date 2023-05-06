using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]

        public ApplicationUser User { get; set; }
        public DateTime DateTime { get; set; }
        public int AttendanceStatusId { get; set; }
        [ForeignKey("AttendanceStatusId")]
        [ValidateNever]
        public AttendanceActions AttendanceActions { get; set; }
    }
}
