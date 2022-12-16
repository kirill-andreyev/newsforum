using Microsoft.Extensions.DependencyInjection;
using NewsForumApi.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constants;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMigrationsDll(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    x => x.MigrationsAssembly(ConstantsStrings.MigrationsAssemblyName));
            });

            return services;
        }
    }
}
