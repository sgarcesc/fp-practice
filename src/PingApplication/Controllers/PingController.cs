using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyNatsClient;

namespace PingApplication.Controllers
{
    [Route("[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        public async Task<string> Get()
        {
            var cnInfo = new ConnectionInfo(new Host("nats.cloudapp.net"))
            {
                AutoReconnectOnFailure = true,
                AutoRespondToPing = true,
                RequestTimeoutMs = (int)TimeSpan.FromMinutes(1).TotalMilliseconds
            };
            using (var client = new NatsClient("request-response", cnInfo))
            {
                client.Connect();
                var response = await client.RequestAsync(subject: "mensaje-emitido", body: "PING_MESSAGE");
                client.Disconnect();
                return response.GetPayloadAsString();
            }
        }
    }
}
