using client.Models;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;

namespace client.Clients;

public class RabbitMQClient
{
    private readonly RabbitMqMessageFactory MqFactory;
    
    public RabbitMQClient()
    {
        MqFactory = new RabbitMqMessageFactory("localhost:5672");
    }
    
    public string? SendMessage()
    {
        var mqClient = MqFactory.CreateMessageQueueClient();
        {
            var replyToMq = mqClient.GetTempQueueName();

            mqClient.Publish(new Message<Hello>(new Hello { Name = "MQ Worker" })
            {
                ReplyTo = replyToMq,
            });

            var responseMsg = mqClient.Get<HelloResponse>(replyToMq);
            mqClient.Ack(responseMsg);

            return responseMsg.ToString();
        }
    }
}