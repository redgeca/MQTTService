using Microsoft.EntityFrameworkCore;

namespace AzureIoTServer.Models
{
    public class IoTDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment environment;

        DbSet<ESP32CamLogs> esp32Logs { get; set; }

        DbSet<TempHumidity> tempHumidities { get; set; }

        public IoTDBContext(DbContextOptions<IoTDBContext> dbContextOptions, IConfiguration configuration, IWebHostEnvironment environment) : base(dbContextOptions)
        {
            _configuration = configuration;
            this.environment = environment;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String? connectionString = _configuration.GetConnectionString("IoTDB");
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
            //        options.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null));

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                options.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null));
        }
    }       
}
