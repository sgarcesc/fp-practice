using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyNatsClient;

namespace PongApplication.Controllers
{
    [Route("[controller]")]
    public class PongController : Controller
    {
        private readonly IMemoryCache _cache;

        public PongController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpGet]
        public int Get()
        {
            _cache.TryGetValue("mensaje-emitido", out int numberOfRequestsProcessed);
            return numberOfRequestsProcessed;
        }
    }
}
