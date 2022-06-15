using Microsoft.AspNetCore.SignalR;
using Services.Contracts;

namespace Services.Implementation.Hubs
{
    public class NotificationHub: Hub<INotificationService>
    {

    }
}