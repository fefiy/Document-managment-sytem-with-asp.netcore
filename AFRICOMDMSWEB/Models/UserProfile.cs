using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string Profiler { get; set; }
        [ForeignKey("Profiler")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
