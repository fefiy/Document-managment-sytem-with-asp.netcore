using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class AttendanceViewModel
    {

        [ValidateNever]
        public Attendance Attendance { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SelectAttendanceActions { get; set; }
        [ValidateNever]
        public List<Attendance> attendances { get; set; }

    }
}
