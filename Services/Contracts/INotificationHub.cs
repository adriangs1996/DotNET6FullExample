using System.Threading.Tasks;
using Domain.Models.Outputs;

namespace Services.Contracts
{
    public interface INotificationHub
    {
        Task TestEntityAdded(TestEntityDetails entity);
        Task TestEntityEdited(TestEntityDetails entity);
        Task TestEntityRemoved(TestEntityDetails entity);
    }
}