using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace IoTFallServer.Models
{
    public class IoTDBContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment environment;

        public IoTDBContext(DbContextOptions<IoTDBContext> dbContextOptions, IConfiguration configuration, IWebHostEnvironment environment) : base(dbContextOptions)
        {
            _configuration = configuration;
            this.environment = environment;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String? connectionString = _configuration.GetConnectionString("IoTDB");
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                options.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null));
        }
    }       
}
