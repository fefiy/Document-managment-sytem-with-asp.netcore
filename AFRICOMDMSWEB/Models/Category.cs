using MessagePack;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class Category
    {

       
        public int Id { get; set; }
        public string Title { get; set; }

        [ValidateNever]
        public int ParentCategoryId { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<Category> SubCategories { get; set; }
      
    }
}
