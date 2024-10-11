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
    public class ListenerQueueMoto : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<ListenerQueueMoto> _logger;
        private readonly IRepositoryMongoMoto _repositoryMongoMoto;
        private readonly string _amqpPort = "AMQP:Port";
        private readonly string _amqpHostName = "AMQP:Hostname";
        private readonly string _amqpActivated = "AMQP:Activated";
        private readonly string _amqpUser = "AMQP:User";
        private readonly string _amqpPassword = "AMQP:Password";
        private readonly string _queueNotify = "queueNotifyMoto";               
        private readonly int _amqpDefaultPort = 5672;        
        private readonly int _amqpPortInUse = 5672;

        public ListenerQueueMoto(IRepositoryMongoMoto repositoryMongoMoto, IConfiguration configuration, ILogger<ListenerQueueMoto> logger)
        {
            _configuration = configuration;
            _logger = logger;
#pragma warning disable CS8604
            _amqpPortInUse = _configuration.GetSection(_amqpPort).Value != null ? int.Parse(_configuration.GetSection(_amqpPort).Value) : _amqpDefaultPort;
            _repositoryMongoMoto = repositoryMongoMoto;
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

                
                var consumerMoto = new EventingBasicConsumer(channel);
                consumerMoto.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"listener: registro moto recebido {message}");

                    var responseMoto = JsonSerializer.Deserialize<MessageMoto>(message);
                    if (responseMoto != null)
                    {
                        SyncMongoDB(responseMoto).GetAwaiter().GetResult();                        
                    }
                };

                channel.BasicConsume(queue: _queueNotify,
                autoAck: true,
                consumer: consumerMoto);                                    
            }                
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }       
        
        private async Task SyncMongoDB(MessageMoto responseMoto)
        {
            if (responseMoto.Action == "add")
            {
                var moto = new MotocycleBike(responseMoto.Identifier, responseMoto.Year, responseMoto.Model, responseMoto.Plate);
                await _repositoryMongoMoto.Add(moto);
                _logger.LogInformation($"listener: adicionado em mongodb moto {responseMoto.Identifier}");
            }
            else if (responseMoto.Action == "update")
            {
                var moto = new MotocycleBike(responseMoto.Identifier, responseMoto.Year, responseMoto.Model, responseMoto.Plate);
                await _repositoryMongoMoto.Update(moto);
                _logger.LogInformation($"listener: atualizado em mongodb moto {responseMoto.Identifier}");
            }
            else if (responseMoto.Action == "remove")
            {
                await _repositoryMongoMoto.Delete(responseMoto.Identifier);
                _logger.LogInformation($"listener: removido de mongodb moto {responseMoto.Identifier}");
            }
        }
    }
}
