
using BB.CR.Providers.Extensions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BB.CR.Rest.MessageQueues
{
    public class MessageProducer : IMessageProducer
    {
        public void Send<T>(T message, string nameAction, ILogger logger)
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMQSetting.HostName,
                UserName = RabbitMQSetting.UserName,
                Password = RabbitMQSetting.Password,
                Port = 15672,
                VirtualHost = "/",
                RequestedHeartbeat = new TimeSpan(60)
            };

            try
            {
                var connection = factory.CreateConnection();

                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: nameAction, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: string.Empty, routingKey: nameAction, basicProperties: null, body: body);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{message}", ex.InnerMessage());
            }
        }
    }
}
