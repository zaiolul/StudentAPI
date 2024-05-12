using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StudentAPI.Data;
using StudentAPI.Models;
using System.Text;

namespace StudentAPI
{
    public class EnrollmentDataCollector : IHostedService
    {
        private Subscriber subscriber;
        private readonly IServiceProvider _serviceProvider;

       
        public EnrollmentDataCollector( IServiceProvider serviceProvider, Subscriber subscriber)
        {
            this.subscriber = subscriber;
            this._serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(ProcessMessage);
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
       
        private bool ProcessMessage(string message, string routingKey)
        {
            Enrollment? enrollment = JsonConvert.DeserializeObject<Enrollment>(message);
            if (enrollment != null)
            {
                Console.WriteLine(enrollment.Title);

            } else
            {
                Console.WriteLine("enrollment null");
            }
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
               
                var _context = scope.ServiceProvider.GetRequiredService<StudentAPIContext>();
                _context.Enrollment.Add(enrollment);
                _context.SaveChanges();
            }
            return true;
        }
    }
}
