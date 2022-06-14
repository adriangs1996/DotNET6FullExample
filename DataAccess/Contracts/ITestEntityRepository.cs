
using Domain.Entities;

namespace DataAccess.Contracts
{
    public interface ITestEntityRepository
    {
        Task<IEnumerable<TestEntity>> GetTestEntitiesAsync();
        Task<TestEntity> GetTestEntity(long testEntityId);
        Task<TestEntity> CreateTestEntity(TestEntity entity);
        Task UpdateTestEntity(TestEntity entity);
        Task DeleteTestEntity(TestEntity entity);
    }
}