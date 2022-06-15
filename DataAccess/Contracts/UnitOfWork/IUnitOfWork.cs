

namespace DataAccess.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ITestEntityRepository TestEntities { get; }
        Task<int> Commit();
    }
}