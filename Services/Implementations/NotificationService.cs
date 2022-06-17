using System;
using System.Threading.Tasks;
using Domain.Models.Outputs;
using Microsoft.AspNetCore.SignalR;
using Services.Contracts;
using Services.Implementation.Hubs;

namespace Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task TestEntityAdded(TestEntityDetails entity)
        {
            Console.WriteLine("Sending Added notification to client");
            await _hubContext.Clients.All.TestEntityAdded(entity);
        }

        public async Task TestEntityEdited(TestEntityDetails entity)
        {
            Console.WriteLine("Sending Edited notification to client");
            await _hubContext.Clients.All.TestEntityEdited(entity);
        }

        public async Task TestEntityRemoved(TestEntityDetails entity)
        {
            Console.WriteLine("Sending Removed notification to client");
            await _hubContext.Clients.All.TestEntityRemoved(entity);
        }
    }
}