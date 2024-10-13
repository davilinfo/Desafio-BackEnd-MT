using Application.Models.ViewModel;
using Domain.Contract.Mongo;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class ListenerQueueLease : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<ListenerQueueLease> _logger;
        private readonly IRepositoryMongoLease _repositoryMongoLease;
        private readonly string _amqpPort = "AMQP:Port";
        private readonly string _amqpHostName = "AMQP:Hostname";
        private readonly string _amqpActivated = "AMQP:Activated";
        private readonly string _amqpUser = "AMQP:User";
        private readonly string _amqpPassword = "AMQP:Password";
        private readonly string _queueNotify = "queueNotifyLease";               
        private readonly int _amqpDefaultPort = 5672;        
        private readonly int _amqpPortInUse = 5672;

        public ListenerQueueLease(IRepositoryMongoLease repositoryMongoLease, IConfiguration configuration, ILogger<ListenerQueueLease> logger)
        {
            _configuration = configuration;
            _logger = logger;
#pragma warning disable CS8604
            _amqpPortInUse = _configuration.GetSection(_amqpPort).Value != null ? int.Parse(_configuration.GetSection(_amqpPort).Value) : _amqpDefaultPort;
            _repositoryMongoLease = repositoryMongoLease;
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

                
                var consumerLease = new EventingBasicConsumer(channel);
                consumerLease.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"listener: registro moto recebido {message}");

                    var responseLease = JsonSerializer.Deserialize<MessageLease>(message);
                    if (responseLease != null)
                    {
                        SyncMongoDB(responseLease).GetAwaiter().GetResult();                        
                    }
                };

                channel.BasicConsume(queue: _queueNotify,
                autoAck: true,
                consumer: consumerLease);                                    
            }                
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }       
        
        private async Task SyncMongoDB(MessageLease responseLease)
        {
            if (responseLease.Action == "add")
            {
                var lease = new Lease(responseLease.DeliverId, responseLease.MotocycleBikeId, responseLease.InitialDate, responseLease.EndDate, responseLease.PreviewEndDate, responseLease.Plan);
                lease.Identifier = responseLease.Identifier;
                lease.Value = responseLease.Value;
                await _repositoryMongoLease.Add(lease);
                _logger.LogInformation($"listener: adicionado em mongodb lease {responseLease.Identifier}");
            }
            else if (responseLease.Action == "update")
            {
                var lease = new Lease(responseLease.DeliverId, responseLease.MotocycleBikeId, responseLease.InitialDate, responseLease.EndDate, responseLease.PreviewEndDate, responseLease.Plan);
                lease.Identifier = responseLease.Identifier;
                lease.DevolutionDate = responseLease.DevolutionDate;
                lease.Value = responseLease.Value;
                await _repositoryMongoLease.Update(lease);
                _logger.LogInformation($"listener: atualizado em mongodb lease {responseLease.Identifier}");
            }            
        }
    }
}
