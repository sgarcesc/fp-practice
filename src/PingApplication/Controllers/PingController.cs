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
        private readonly INatsClient _client;
        public PingController(INatsClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var response = await _client.RequestAsync(subject: "mensaje-emitido", body: "PING_MESSAGE");
            return response.GetPayloadAsString();
        }
    }
}
