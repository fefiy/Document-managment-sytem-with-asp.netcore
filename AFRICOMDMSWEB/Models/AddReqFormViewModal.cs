using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AFRICOMDMSWEB.Models
{
    public class AddReqFormViewModal
    {
        [ValidateNever]
        public RequestFile RequestFiles { get; set; }
        public Request Requests { get; set; }
        [ValidateNever]
        public IEnumerable<RequiestForm> RequiestForms { get; set; }


    }
}
