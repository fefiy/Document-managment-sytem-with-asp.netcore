using System.ComponentModel.DataAnnotations;

namespace AFRICOMDMSWEB.Models
{
    public class ReqType
    {
        [Key]
        public int ReqTypeId { get; set; }

        public string Name { get; set; }
    }
}
