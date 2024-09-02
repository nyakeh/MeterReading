using MeterReader.Models;
using MeterReader.Data;

namespace MeterReader.Services
{
    public class MeterReadingService
    {
        private readonly ISmartMeterRepository _smartMeterRepository;

        public MeterReadingService(ISmartMeterRepository smartMeterRepository)
        {
            _smartMeterRepository = smartMeterRepository;
        }

        public UploadMeterReadingsResponse ProcessMeterReadings(IFormFile file)
        {
            var response = new UploadMeterReadingsResponse
            {
                SuccessfulReadings = 0,
                FailedReadings = 0,
                InvalidMeterReadings = new List<InvalidMeterReading>()
            };

            var validMeterReadings = new List<MeterReading>();
            var invalidMeterReadings = new List<InvalidMeterReading>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                reader.ReadLine();

                string row;
                while ((row = reader.ReadLine()) != null)
                {
                    var values = row.Split(',');

                    if(InputValidator.TryParse(row, out MeterReadingDto input))
                    {
                        var existingRead = validMeterReadings.FirstOrDefault(r => r.Account.Id == input.AccountId);
                        if (existingRead != null)
                        {
                            if (existingRead.MeterReadingDateTime > input.MeterReadingDateTime)
                            {
                                response.InvalidMeterReadings.Add(new InvalidMeterReading(row, "Meter reading date is older than existing reading."));
                                response.FailedReadings++;
                                continue;
                            }
                            else
                            {
                                response.InvalidMeterReadings.Add(new InvalidMeterReading(string.Empty, $"Row replaced by newer meter read date:'{existingRead.MeterReadingDateTime.ToString("g")}' for AccountId: '{input.AccountId}'."));
                                response.FailedReadings++;
                                existingRead.MeterReadingDateTime = input.MeterReadingDateTime;
                                existingRead.MeterReadValue = input.MeterReadValue;
                                continue;
                            }
                        }

                        var account = _smartMeterRepository.GetAccountById(input.AccountId);
                        if(account == null)
                        {
                            response.InvalidMeterReadings.Add(new InvalidMeterReading(row, $"Account Id:'{input.AccountId}' not found."));
                            response.FailedReadings++;
                            continue;
                        }

                        validMeterReadings.Add(new MeterReading
                        {
                            Account = account!,
                            MeterReadingDateTime = input.MeterReadingDateTime,
                            MeterReadValue = input.MeterReadValue
                        });
                        response.SuccessfulReadings++;
                    }
                    else
                    {
                        response.InvalidMeterReadings.Add(new InvalidMeterReading(row, "Invalid input format."));
                        response.FailedReadings++;
                    }
                }
            }

            _smartMeterRepository.BulkAddMeterReadings(validMeterReadings);

            return response;
        }

        public List<MeterReadingDto> GetMeterReadings()
        {
            var meterReadings = _smartMeterRepository.GetAllMeterReadings();
            return meterReadings.Select(reading => reading.ToMeterReadingInput()).ToList();
        }

        public List<Account> GetAccounts()
        {
            return _smartMeterRepository.GetAllAccounts();
        }

        public void BulkDeleteMeterReadings()
        {
            _smartMeterRepository.BulkDeleteMeterReadings();
        }

        public List<MeterReadingDto> MeterReadingsByAccount(int accountId)
        {
            var meterReadings = _smartMeterRepository.GetMeterReadingsByAccount(accountId);
            return meterReadings.Select(reading => reading.ToMeterReadingInput()).ToList();
        }
    }
}
