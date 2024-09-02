using MeterReader.Models;
using System.Globalization;

namespace MeterReader.Services
{
    public static class InputValidator
    {
        private const int AccountIdIndex = 0;
        private const int DateIndex = 1;
        private const int MeterReadIndex = 2;

        public static bool TryParse(string input, out MeterReadingDto smartMeterInput)
        {
            smartMeterInput = null!;
            var values = input.Split(',');

            if (values.Length < 3)
            {
                return false;
            }

            if (!int.TryParse(values[AccountIdIndex], out int accountId))
            {
                return false;
            }

            if (!DateTime.TryParseExact(values[DateIndex], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime meterReadingDateTime))
            {
                return false;
            }

            if (!int.TryParse(values[MeterReadIndex], out int meterReadValue))
            {
                return false;
            }

            smartMeterInput = new MeterReadingDto
            {
                AccountId = accountId,
                MeterReadingDateTime = meterReadingDateTime,
                MeterReadValue = meterReadValue
            };

            return true;
        }
    }
}
