using System.Threading.Tasks;
using DataAccess.Contracts;
using DataAccess.Contracts.UnitOfWork;

namespace DataAccess.Implementations.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        public ITestEntityRepository TestEntities { get; private set; }
        private readonly TestContext _context;

        public UnitOfWork(TestContext context)
        {
            _context = context;
            TestEntities = new TestEntityRepository(_context);
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }
    }

}