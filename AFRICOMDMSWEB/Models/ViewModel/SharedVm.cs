using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class SharedVm
    {
        [ValidateNever]
        public FileShared sharedFiles { get; set; }

        public int produtId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RecieverList { get; set; }

        [ValidateNever]
        public IEnumerable<Department> Depatment { get; set; }

        public int DepartmetId { get; set; }


        public bool IsSharedable { get; set; }

        public bool IsUpdatable { get; set; }

        public bool Isdowloadble { get; set; }

    }
}
