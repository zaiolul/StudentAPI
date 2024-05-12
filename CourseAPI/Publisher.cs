
using RabbitMQ.Client;
using System.Text;


namespace CourseAPI
{

    public class Publisher
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        string exchange;
        string routingKey;
 
        public Publisher(string hostname, string username, string password, string exchange, string routingKey) 
        {
            Console.WriteLine("Create Publisher");
            this.exchange = exchange;
            this.routingKey = routingKey;
            factory = new ConnectionFactory { HostName = hostname, UserName = username, Password = password};
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);
        }

        public void Publish(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            var props = channel.CreateBasicProperties();
            props.Persistent = true;
            props.Expiration = "10000";
            channel.BasicPublish(exchange: exchange,
                     routingKey: routingKey,
                     basicProperties: props,
                     body: body);
        }
    }
}
