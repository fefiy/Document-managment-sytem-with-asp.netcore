
using System.ComponentModel.DataAnnotations;

namespace AFRICOMDMSWEB.Models
{
    public class Department
    {
        [Key]
        public int DepId { get; set; }

        public string Name { get; set; }    


    }
}
