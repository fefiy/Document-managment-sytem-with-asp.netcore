using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }

        public int DepId { get; set; }
        [ForeignKey("DepId")]
        [ValidateNever]
        public Department Department { get; set; }

        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        [ValidateNever]
        public string Role { get; set; }
        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }

        [ValidateNever]
        [NotMapped]
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
    }
}
