using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Azure.Messaging.ServiceBus;
using Domain.Models;
using Services.Contracts;

namespace TestClientBlazor.Service
{
    public class AzureSubscriberService : IHostedService
    {
        private readonly INotificationService _notifications;
        private ServiceBusClient? _client;
        private ServiceBusProcessor? _processor;

        private const string ConnectionString =
            "Endpoint=sb://nft-servicebus-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aCOMWo3cGQK7oAdRC782T5d7AXaFOix8FMIZ4Qwu874=";

        private const string QueueName = "message";

        public AzureSubscriberService(INotificationService notifications)
        {
            _notifications = notifications;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Azure Subscriber");
            _client = new ServiceBusClient(ConnectionString);
            _processor = _client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());
            Console.WriteLine("Connected");
            try
            {
                // Add handler to process incoming messages 
                _processor.ProcessMessageAsync += MessageHandler;
                // Add handler to process errors
                _processor.ProcessErrorAsync += ErrorHandler;

                await _processor.StartProcessingAsync(stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Cleaning IHostedService");
            if (_processor is not null)
            {
                await _processor.DisposeAsync();
            }

            if (_client is not null)
            {
                await _client.DisposeAsync();
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageHandler(ProcessMessageEventArgs arg)
        {
            string body = arg.Message.Body.ToString();
            Console.WriteLine(body);
            try
            {
                // Deserialize given body
                AzureMessageModel? message = JsonSerializer.Deserialize<AzureMessageModel>(body);
                if (message is null)
                {
                    Console.WriteLine(body);
                }
                else
                {
                    switch (message.Topic)
                    {
                        case Topic.CreatedTestEntity:
                        {
                            await _notifications.TestEntityAdded(message.Entity);
                            break;
                        }
                        case Topic.DeletedTestEntity:
                        {
                            await _notifications.TestEntityRemoved(message.Entity);
                            break;
                        }
                        case Topic.EditedTestEntity:
                        {
                            await _notifications.TestEntityEdited(message.Entity);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await arg.CompleteMessageAsync(arg.Message);
            }
        }
    }
}