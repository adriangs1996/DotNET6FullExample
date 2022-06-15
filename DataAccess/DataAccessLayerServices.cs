
using DataAccess.Contracts;
using DataAccess.Contracts.UnitOfWork;
using DataAccess.Implementations.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DataAccessServices
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            services.AddDbContext<TestContext>(options => options.UseInMemoryDatabase("TestDatabase"));
            services.AddScoped<ITestEntityRepository, TestEntityRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}