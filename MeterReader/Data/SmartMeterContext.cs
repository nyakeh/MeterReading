using Microsoft.EntityFrameworkCore;
using MeterReader.Models;

namespace MeterReader.Data
{
    public class SmartMeterContext : DbContext
    {
        public SmartMeterContext(DbContextOptions<SmartMeterContext> options)
            : base(options)
        {
        }

        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
