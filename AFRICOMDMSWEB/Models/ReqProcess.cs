using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFRICOMDMSWEB.Models
{
    public class ReqProcess
    {
        [Key]
        public int Id { get; set; }

        public string RoleId { get; set; }
        [ForeignKey("RoleId")] 

        public string? ParentRoleId { get; set; }

        public int ReqtypeId { get; set; }

        [ForeignKey("ReqtypeId")]

        public ReqType ReqType { get; set; }    

    }
}
