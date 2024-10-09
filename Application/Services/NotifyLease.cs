﻿using Application.Interface;
using Application.Models.Response;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text.Json;

namespace Application.Services
{
    public class NotifyLease : INotify<ResponseLease>
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<NotifyLease> _logger;
        private readonly string _amqpPort = "AMQP:Port";
        private readonly string _amqpHostName = "AMQP:Hostname";
        private readonly string _amqpUser = "AMQP:User";
        private readonly string _amqpPassword = "AMQP:Password";        
        private readonly string _queueNotify = "queueNotifyLease";
        private readonly string _messageToSend = "Mensagem:";
        private readonly string _messageSent = "Mensagem enviada:";
        private readonly string _assemblyName = "Application";
        private readonly int _amqpDefaultPort = 5672;
        private readonly int _evtId = 21000;
        private readonly int _amqpPortInUse = 5672;

        public NotifyLease(IConfiguration configuration, ILogger<NotifyLease> logger)
        {
            _configuration = configuration;
            _logger = logger;
#pragma warning disable CS8604
            _amqpPortInUse = _configuration.GetSection(_amqpPort).Value != null ? int.Parse(_configuration.GetSection(_amqpPort).Value) : _amqpDefaultPort;
#pragma warning restore CS8604
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection(_amqpHostName).Value,
                Port = _amqpPortInUse,
                UserName = _configuration.GetSection(_amqpUser).Value,
                Password = _configuration.GetSection(_amqpPassword).Value
            };
        }
        public void NotifyMessage(ResponseLease message)
        {
            var serialized = JsonSerializer.Serialize<ResponseLease>(message);
            var eventid = new EventId(_evtId, _assemblyName);
            _logger.LogInformation(eventid, $"{_messageToSend} {serialized}");

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(_queueNotify,
                        durable: false,
                        exclusive: false,
                        autoDelete: false);
                    
                    var byteMessage = System.Text.Encoding.UTF8.GetBytes(serialized);
                    channel.BasicPublish(exchange: "",
                        routingKey: _queueNotify,
                        basicProperties: null,
                        body: byteMessage);
                    
                    _logger.LogInformation(eventid, $"{_messageSent} {serialized}");
                }
                connection.Close();
            }
        }
    }
}