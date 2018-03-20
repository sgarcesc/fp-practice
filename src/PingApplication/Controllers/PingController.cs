using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyNatsClient;

namespace PingApplication.Controllers
{
    [Route("[controller]")]
    public class PingController : Controller
    {
        [HttpPut]
        public async Task<string> Put()
        {
            //Configure the NATS client connection
            var cnInfo = new ConnectionInfo(new Host("nats.cloudapp.net"));
            var client = new NatsClient("request-response", cnInfo);
            client.Connect();

            var response = await client.RequestAsync("mensaje-emitido", "PING_MESSAGE");
            return response.GetPayloadAsString();
        }
    }
}
