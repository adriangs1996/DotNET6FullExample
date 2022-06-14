using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }

        public DbSet<TestContext> TestItems { get; set; } = null!;
    }
}
