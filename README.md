# mqtest

How to run:

1. Start RabbitMQ: `docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management` 
2. Start the client: ` dotnet run --project client`
3. Browse to https://localhost:7200/request
