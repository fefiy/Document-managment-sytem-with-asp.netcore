using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class AprrovalAnswer
    {
        [Key]
        public int id { get; set; }
        public int ApproveId { get; set; }
        [ForeignKey("ApproveId")]
        public Request ApprovedRequest { get; set; }

        public int Answer { get; set; }
    }
}
