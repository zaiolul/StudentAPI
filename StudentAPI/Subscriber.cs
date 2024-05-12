using Microsoft.CodeAnalysis.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace StudentAPI
{
    public class Subscriber 
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        string queueName;
        string hostname;
        string routingKey;
        string exchange;
        public Subscriber(string hostname, string username, string password, string exchange, string queueName, string routingKey) 
        {
            Console.WriteLine("Create Subscriber");
            this.exchange = exchange;
            this.queueName = queueName;
            this.routingKey = routingKey;
            this.hostname = hostname;
            factory = new ConnectionFactory { HostName = hostname, UserName = username, Password = password };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);

            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queue: queueName, exchange: exchange, routingKey: routingKey);

            channel.BasicQos(0, 1, false);
        }
        public void Subscribe(Func<string, string, bool> callback)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (callback.Invoke(message, ea.RoutingKey))
                {
                    channel.BasicAck(ea.DeliveryTag, true);
                }
            };
            channel.BasicConsume(queue: queueName,
                     autoAck: false,
                     consumer: consumer);
        }
    }
}
