using System.ComponentModel.DataAnnotations;

namespace MeterReader.Models
{
    public class MeterReading
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Account Account { get; set; }

        [Required]
        public DateTime MeterReadingDateTime { get; set; }

        [Required]
        public int MeterReadValue { get; set; }

        public MeterReadingDto ToMeterReadingInput()
        {
            return new MeterReadingDto
            {
                AccountId = Account.Id,
                MeterReadingDateTime = MeterReadingDateTime,
                MeterReadValue = MeterReadValue
            };
        }
    }
}
