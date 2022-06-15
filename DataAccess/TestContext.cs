using DataAccess.Contracts;
using DataAccess.Contracts.UnitOfWork;
using DataAccess.Implementations.UnitOfWork;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }

        public DbSet<TestEntity> TestItems { get; set; } = null!;
    }

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
