namespace MeterReader.Models
{
    public class UploadMeterReadingsResponse
    {
        public required int SuccessfulReadings { get; set; }
        public required int FailedReadings { get; set; }
        public required List<InvalidMeterReading> InvalidMeterReadings { get; set; }
    }

}
