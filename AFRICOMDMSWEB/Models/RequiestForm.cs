using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing.Constraints;

namespace AFRICOMDMSWEB.Models
{
    public class RequiestForm
    {
        public int Id { get; set; }
         
        public string Title { get; set; }
        [ValidateNever]
        public string FileUrl { get; set; }
        [ValidateNever]

        public byte[] Filecontent { get; set; }
        [ValidateNever]

        public string FileName { get; set; }
        [ValidateNever]

        public string FileContentType { get; set; }
    }
}
