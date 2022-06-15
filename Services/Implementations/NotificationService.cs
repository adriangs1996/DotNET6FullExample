using Domain.Models.Outputs;
using Microsoft.AspNetCore.SignalR;
using Services.Contracts;
using Services.Implementation.Hubs;

namespace Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationService> hubContext;

        public NotificationService(IHubContext<NotificationHub, INotificationService> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task TestEntityAdded(TestEntityDetails entity)
        {
            await hubContext.Clients.All.TestEntityAdded(entity);
        }

        public async Task TestEntityEdited(TestEntityDetails entity)
        {
            await hubContext.Clients.All.TestEntityEdited(entity);
        }

        public async Task TestEntityRemoved(TestEntityDetails entity)
        {
            await hubContext.Clients.All.TestEntityRemoved(entity);
        }
    }
}