using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class TakeActionViewModel
    {
        [ValidateNever]
        public RequestAction requestAction { get; set; }
        [ValidateNever]

        public Transition transition { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ActionsType { get; set; }
    
        public Request request { get; set; }
        [ValidateNever]
        public int RequestID { get; set; }

        public RequestFile requestFile { get; set; }

        public ReqComment reqComment { get; set; }

        public bool writeComent { get; set; }

        public ReqAnswerFiles ReqAnswerFiles { get; set; }
    }
}
