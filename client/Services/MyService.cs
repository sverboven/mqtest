using client.Models;
using ServiceStack;

namespace client.Services
{
    public class MyService : Service
    {
        public object Any(Hello request) => new HelloResponse
        {
            Result = $"Hello, {request.Name}!"
        };
    }
}