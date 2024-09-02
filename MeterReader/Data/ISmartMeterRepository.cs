using MeterReader.Models;

namespace MeterReader.Data
{
    public interface ISmartMeterRepository
    {
        void AddAccount(Account account);
        void AddMeterReading(MeterReading meterReading);
        void BulkAddAccounts(List<Account> accounts);
        void BulkAddMeterReadings(List<MeterReading> meterReadings);
        Account? GetAccountById(int accountId);
        MeterReading GetMeterReadingById(int meterReadingId);
        List<Account> GetAllAccounts();
        List<MeterReading> GetAllMeterReadings();
        void BulkDeleteMeterReadings();
        List<MeterReading> GetMeterReadingsByAccount(int accountId);
    }
}