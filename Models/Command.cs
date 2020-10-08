using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class Command{
        [Key]//le decimos a EF que esto será una PK
        public int Id { get; set; }
        
        [Required]//no null
        [MaxLength(250)]//varchar(250)
        public string HowTo { get; set; }

        [Required]//no null
        public string Line { get; set; }

        [Required]//no null
        public string Platform { get; set; }
    }
}