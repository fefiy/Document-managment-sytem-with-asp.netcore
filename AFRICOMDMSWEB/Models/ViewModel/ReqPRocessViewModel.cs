using Microsoft.AspNetCore.Mvc.Rendering;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class ReqPRocessViewModel
    {
        public ReqProcess reqProcess { get; set; }

        public IEnumerable<SelectListItem> selectRequestType {get; set ;}
        public IEnumerable<SelectListItem> selectPearentRole{get; set ;}
    }
}
