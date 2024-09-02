using Microsoft.AspNetCore.Mvc;
using MeterReader.Models;
using MeterReader.Services;
using MeterReader.Data;

namespace MeterReader.Controllers
{
    [ApiController]
    [Route("")]
    public class MeterReadingController : ControllerBase
    {
        private readonly MeterReadingService _meterReadingService;

        public MeterReadingController(ISmartMeterRepository smartMeterRepository)
        {
            _meterReadingService = new MeterReadingService(smartMeterRepository);
        }

        [HttpPost("meter-reading-uploads", Name = "meter-reading-uploads")]
        public IActionResult UploadMeterReadings([FromForm] IFormFileCollection meterReadingFile)
        {
            try
            {
                var file = meterReadingFile.FirstOrDefault();
                if (file == null)
                {
                    return BadRequest("No file uploaded.");
                }

                var response = _meterReadingService.ProcessMeterReadings(file);

                return Ok(response);
            }
            catch
            {
                return BadRequest("An error occurred while processing the file.");
            }
        }

        [HttpGet("account/{accountId}/meter-readings", Name = "get-meter-readings-by-account")]
        public List<MeterReadingDto> GetMeterReadingsByAccount(int accountId)
        {
            return _meterReadingService.MeterReadingsByAccount(accountId);
        }
        
        [HttpGet("meter-readings", Name = "meter readings")]
        public List<MeterReadingDto> GetMeterReadings()
        {
            return _meterReadingService.GetMeterReadings();
        }

        [HttpGet("accounts", Name = "accounts")]
        public List<Account> GetAccounts()
        {
            return _meterReadingService.GetAccounts();
        }

        [HttpDelete("meter-readings", Name = "delete-all-meter-readings")]
        public IActionResult DeleteAllMeterReadings(int id)
        {            
            _meterReadingService.BulkDeleteMeterReadings();
            return Ok();
        }
    }
}
