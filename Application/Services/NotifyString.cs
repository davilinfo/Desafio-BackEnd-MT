using Application.Interface;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Application.Services
{
    public class NotifyString : INotify<string>
    {
        private readonly IConfiguration _configuration;        
        private readonly IConnectionFactory _connectionFactory;
        private readonly string _amqpPort = "AMQP:Port";
        private readonly string _amqpHostName = "AMQP:Hostname";
        private readonly string _amqpActivated = "AMQP:Activated";
        private readonly string _amqpUser = "AMQP:User";
        private readonly string _amqpPassword = "AMQP:Password";
        private readonly string _queueNotify = "queueNotifyMessage";
        private readonly int _amqpDefaultPort = 5672;
        public NotifyString(IConfiguration configuration) 
        { 
            _configuration = configuration;
#pragma warning disable CS8604
            var amqpPort = _configuration.GetSection(_amqpPort).Value != null ? int.Parse(_configuration.GetSection(_amqpPort).Value) : _amqpDefaultPort;
#pragma warning restore CS8604
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection(_amqpHostName).Value,
                Port = amqpPort,
                UserName = _configuration.GetSection(_amqpUser).Value,
                Password = _configuration.GetSection(_amqpPassword).Value
            };
        }
        public void NotifyMessage(string message)
        {
            if (_configuration.GetSection(_amqpActivated).Value == "true")
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(_queueNotify,
                            durable: false,
                            exclusive: false,
                            autoDelete: false);

                        var byteMessage = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "",
                            routingKey: _queueNotify,
                            basicProperties: null,
                            body: byteMessage);
                    }
                    connection.Close();
                }
            }
        }
    }
}
