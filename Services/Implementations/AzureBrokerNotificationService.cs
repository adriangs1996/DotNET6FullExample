using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Domain.Models;
using Domain.Models.Outputs;
using Services.Contracts;

namespace Services.Implementation
{
    public class AzureBrokerNotificationService : INotificationService, IAsyncDisposable
    {
        private const string ConnectionString = "Endpoint=sb://nft-servicebus-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aCOMWo3cGQK7oAdRC782T5d7AXaFOix8FMIZ4Qwu874=";

        private const string QueueName = "message";

        private readonly ServiceBusSender _sender;
        private readonly ServiceBusClient _client;

        public AzureBrokerNotificationService()
        {
            _client = new ServiceBusClient(ConnectionString);
            _sender = _client.CreateSender(QueueName);
        }

        public async Task TestEntityAdded(TestEntityDetails entity)
        {
            AzureMessageModel message = new(Topic.CreatedTestEntity, entity);
            string jsonMessage = JsonSerializer.Serialize(message);
            Console.WriteLine($"Sending {jsonMessage} to Azure Broker");
            await _sender.SendMessageAsync(new ServiceBusMessage(jsonMessage));
        }

        public async Task TestEntityEdited(TestEntityDetails entity)
        {
            AzureMessageModel message = new(Topic.EditedTestEntity, entity);
            string jsonMessage = JsonSerializer.Serialize(message);
            Console.WriteLine($"Sending {jsonMessage} to Azure Broker");
            await _sender.SendMessageAsync(new ServiceBusMessage(jsonMessage));
        }

        public async Task TestEntityRemoved(TestEntityDetails entity)
        {
            AzureMessageModel message = new(Topic.DeletedTestEntity, entity);
            string jsonMessage = JsonSerializer.Serialize(message);
            Console.WriteLine($"Sending {jsonMessage} to Azure Broker");
            await _sender.SendMessageAsync(new ServiceBusMessage(jsonMessage));
        }

        public async ValueTask DisposeAsync()
        {
            await _sender.DisposeAsync();
            await _client.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}