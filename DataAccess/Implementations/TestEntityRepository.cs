using DataAccess.Implementations;
using Domain.Entities;

namespace DataAccess.Contracts
{
    public class TestEntityRepository : Repository<TestEntity, long>, ITestEntityRepository
    {
        public TestEntityRepository(TestContext context) : base(context)
        {
        }
    }
}