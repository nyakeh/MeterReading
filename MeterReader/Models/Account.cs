using System.ComponentModel.DataAnnotations;

namespace MeterReader.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
    }
}
