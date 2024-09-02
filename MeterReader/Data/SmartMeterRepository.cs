using MeterReader.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReader.Data
{
    public class SmartMeterRepository : ISmartMeterRepository
    {
        private readonly SmartMeterContext _context;

        public SmartMeterRepository(SmartMeterContext context)
        {
            _context = context;
        }

        public Account? GetAccountById(int accountId)
        {
            return _context.Accounts.FirstOrDefault(account => account.Id == accountId);
        }

        public MeterReading GetMeterReadingById(int MeterReadingId)
        {
            return _context.MeterReadings.Include(meterReading => meterReading.Account).First(meterReading => meterReading.Id == MeterReadingId);
        }
                
        public List<MeterReading> GetMeterReadingsByAccount(int accountId)
        {
            return _context.MeterReadings.Include(meterReading => meterReading.Account).Where(meterReading => meterReading.Account.Id == accountId).ToList();
        }

        public List<MeterReading> GetAllMeterReadings()
        {
            return _context.MeterReadings.Include(meterReading => meterReading.Account).ToList();
        }

        public List<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public void BulkAddMeterReadings(List<MeterReading> meterReadings)
        {
            _context.MeterReadings.AddRange(meterReadings);
            _context.SaveChanges();
        }

        public void AddMeterReading(MeterReading meterReading)
        {
            _context.MeterReadings.Add(meterReading);
            _context.SaveChanges();
        }

        public void BulkAddAccounts(List<Account> accounts)
        {
            _context.Accounts.AddRange(accounts);
            _context.SaveChanges();
        }

        public void AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public void BulkDeleteMeterReadings()
        {
            _context.MeterReadings.RemoveRange(_context.MeterReadings);
            _context.SaveChanges();
        }
    }
}
