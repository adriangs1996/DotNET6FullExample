using Domain.Models.Outputs;

namespace Services.Contracts
{
    public interface INotificationService
    {
        Task TestEntityAdded(TestEntityDetails entity);
        Task TestEntityEdited(TestEntityDetails entity);
        Task TestEntityRemoved(TestEntityDetails entity);
    }
}