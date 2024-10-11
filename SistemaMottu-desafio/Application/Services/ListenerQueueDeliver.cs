using Application.Models.ViewModel;
using Domain.Contract.Mongo;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class ListenerQueueDeliver : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<ListenerQueueDeliver> _logger;
        private readonly IRepositoryMongoDeliver _repositoryMongoDeliver;
        private readonly string _amqpPort = "AMQP:Port";
        private readonly string _amqpHostName = "AMQP:Hostname";
        private readonly string _amqpActivated = "AMQP:Activated";
        private readonly string _amqpUser = "AMQP:User";
        private readonly string _amqpPassword = "AMQP:Password";
        private readonly string _queueNotify = "queueNotifyDeliver";               
        private readonly int _amqpDefaultPort = 5672;        
        private readonly int _amqpPortInUse = 5672;

        public ListenerQueueDeliver(IRepositoryMongoDeliver repositoryMongoDeliver, IConfiguration configuration, ILogger<ListenerQueueDeliver> logger)
        {
            _configuration = configuration;
            _logger = logger;
#pragma warning disable CS8604
            _amqpPortInUse = _configuration.GetSection(_amqpPort).Value != null ? int.Parse(_configuration.GetSection(_amqpPort).Value) : _amqpDefaultPort;
            _repositoryMongoDeliver = repositoryMongoDeliver;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection(_amqpHostName).Value,
                Port = _amqpPortInUse,
                UserName = _configuration.GetSection(_amqpUser).Value,
                Password = _configuration.GetSection(_amqpPassword).Value
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (bool.Parse(_configuration.GetSection(_amqpActivated).Value) == true)
            {
                var connection = _connectionFactory.CreateConnection();
                var channel = connection.CreateModel();
                    
                channel.QueueDeclare(_queueNotify,
                    durable: false,
                    exclusive: false,
                    autoDelete: false);

                
                var consumerDeliver = new EventingBasicConsumer(channel);
                consumerDeliver.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"listener: registro moto recebido {message}");

                    var responseDeliver = JsonSerializer.Deserialize<MessageDeliver>(message);
                    if (responseDeliver != null)
                    {
                        SyncMongoDB(responseDeliver).GetAwaiter().GetResult();                        
                    }
                };

                channel.BasicConsume(queue: _queueNotify,
                autoAck: true,
                consumer: consumerDeliver);                                    
            }                
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }       
        
        private async Task SyncMongoDB(MessageDeliver responseDeliver)
        {
            if (responseDeliver.Action == "add")
            {
                var deliver = new Deliver(responseDeliver.Identifier, responseDeliver.Name, responseDeliver.UniqueIdentifier, responseDeliver.Birthday, responseDeliver.DriverLicenseNumber, responseDeliver.DriverLicenseType, responseDeliver.DriverLicenseImageS3);
                await _repositoryMongoDeliver.Add(deliver);
                _logger.LogInformation($"listener: adicionado em mongodb deliver {responseDeliver.Identifier}");
            }
            else if (responseDeliver.Action == "update")
            {
                var deliver = new Deliver(responseDeliver.Identifier, responseDeliver.Name, responseDeliver.UniqueIdentifier, responseDeliver.Birthday, responseDeliver.DriverLicenseNumber, responseDeliver.DriverLicenseType, responseDeliver.DriverLicenseImageS3);
                await _repositoryMongoDeliver.Update(deliver);
                _logger.LogInformation($"listener: atualizado em mongodb deliver {responseDeliver.Identifier}");
            }            
        }
    }
}
