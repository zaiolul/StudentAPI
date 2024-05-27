using ClassLibrary;
using MassTransit;

namespace StudentAPI
{
    public class MsgConsumer : IConsumer<MTMsg>
    {
        readonly ILogger<MsgConsumer> logger;
        public MsgConsumer(ILogger<MsgConsumer> logger)
        {
            Console.WriteLine("Test");
            this.logger = logger;
        }
        public Task Consume(ConsumeContext<MTMsg> context)
        {
            var recv = new MTMsg
            {
                Timestamp = context.Message.Timestamp,
                Content = context.Message.Content,
            };

            logger.LogInformation("testas");
            logger.LogInformation("Žinutė persiųsta naudojant MassTransit:\nLaikas: {0}\n{1}", recv.Timestamp, recv.Content);
            return Task.CompletedTask;

        }

    }
   
}
