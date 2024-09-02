namespace MeterReader.Data
{
    public static class DatabaseExtension
    {
        public static void CreateDbIfNotExists(this IHost serviceHost)
        {
            {
                using (var scope = serviceHost.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<SmartMeterContext>();
                    context.Database.EnsureCreated();
                    DatabaseSeeder.Initialise(context);
                }
            }
        }
    }
}
