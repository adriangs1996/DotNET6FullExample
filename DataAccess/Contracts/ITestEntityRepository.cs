
using Domain.Entities;

namespace DataAccess.Contracts
{
    public interface ITestEntityRepository: IRepository<TestEntity, long>
    {
    }
}