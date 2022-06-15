using Domain.Entities;
using Domain.Models.Inputs;

namespace Services.Contracts
{
    public interface IApplicationService
    {
        Task<TestEntity> GetTestEntityById(long id);
        Task<IEnumerable<TestEntity>> GetAllTestEntities();
        Task<TestEntity> UpdateEntityIfPresent(long id, TestEntityInDto form);
        Task<TestEntity> CreateTestEntity(TestEntityInDto form);
        Task<TestEntity> DeleteTestEntity(long id);
    }
}