using Microsoft.EntityFrameworkCore;

namespace HiringTest.Entities
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
