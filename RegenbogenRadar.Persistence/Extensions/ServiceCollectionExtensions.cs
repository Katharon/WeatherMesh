namespace RegenbogenRadar.Persistence.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RegenbogenRadar.Persistence.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            var cs = config.GetConnectionString("WeatherStationDb");
            services.AddDbContextPool<WeatherStationDbContext>(opt =>
            {
                opt.UseSqlServer(cs, sql =>
                {
                    sql.EnableRetryOnFailure();
                    sql.MigrationsAssembly(typeof(WeatherStationDbContext).Assembly.FullName);
                });
            });

            // Für Background-Worker (z. B. Ingestion):
            services.AddPooledDbContextFactory<WeatherStationDbContext>(opt =>
            {
                opt.UseSqlServer(cs);
            });

            return services;
        }
    }
}
