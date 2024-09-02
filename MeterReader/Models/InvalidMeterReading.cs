namespace MeterReader.Models
{
    public class InvalidMeterReading
    {
        public string RawInput { get; set; }
        public string Reason { get; set; }

        public InvalidMeterReading(string rawInput, string reason)
        {
            RawInput = rawInput;
            Reason = reason;
        }
    }
}
