using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class CatagoryVm
    {
        public Category category { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> subCatagories { get; set; }
        [ValidateNever]
        public IEnumerable<Category> categories { get; set; }
    }
}
