namespace RegenbogenRadar.Persistence.Data
{
    using Microsoft.EntityFrameworkCore;
    using RegenbogenRadar.Domain.WeatherStation;

    public class WeatherStationDbContext : DbContext
    {
        public WeatherStationDbContext(DbContextOptions<WeatherStationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherStation> WeatherStations => Set<WeatherStation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
