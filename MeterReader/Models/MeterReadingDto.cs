namespace MeterReader.Models
{
    public class MeterReadingDto
    {
        public required int AccountId { get; set; }
        public required DateTime MeterReadingDateTime { get; set; }
        public required int MeterReadValue { get; set; }
    }
}
