using client.Clients;
using client.Model;
using Microsoft.AspNetCore.Mvc;

namespace client.Controller;

[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly ILogger<RequestController> _logger;
    private readonly RabbitMQClient _mqClient;

    public RequestController(ILogger<RequestController> logger)
    {
        _logger = logger;
        _mqClient = new RabbitMQClient();
    }

    [HttpGet(Name = "request")]
    public IEnumerable<Response> Get()
    {
        var res = new Response();
        
        res.text = _mqClient.SendMessage();
        
        return new Response[] { res };
    }
}