using MeterReader.Models;

namespace MeterReader.Data
{
    public static class DatabaseSeeder
    {
        private const string TestAccountsFilePath = "Data\\Test_Accounts.txt";

        public static void Initialise(SmartMeterContext context)
        {
            if (context.Accounts.Any())
            {
                return;
            }

            var accounts = ParseAccountsFromFile(TestAccountsFilePath);

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }

        public static List<Account> ParseAccountsFromFile(string filePath)
        {
            List<Account> accounts = new List<Account>();

            string[] rows = File.ReadAllLines(filePath);

            foreach (string row in rows.Skip(1))
            {
                string[] columns = row.Split('|');

                Account account = new Account
                {
                    Id = int.Parse(columns[0]),
                    FirstName = columns[1],
                    LastName = columns[2]
                };

                accounts.Add(account);
            }

            return accounts;
        }
    }
}
