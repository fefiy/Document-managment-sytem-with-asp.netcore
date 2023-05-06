using System.ComponentModel.DataAnnotations;


namespace AFRICOMDMSWEB.Models
{
    public class Actions
    {
        [Key]
        public  int id { get; set; }

        public string Name { get; set; }
    }
}
