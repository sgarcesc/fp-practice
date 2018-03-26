using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyNatsClient;

namespace PongApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cnInfo = new ConnectionInfo(new Host("nats.cloudapp.net"))
            {
                AutoReconnectOnFailure = true,
                AutoRespondToPing = true,
                RequestTimeoutMs = (int)TimeSpan.FromMinutes(5).TotalMilliseconds
            };
            var client = new NatsClient("request-response", cnInfo);
            client.Connect();
            services.AddSingleton<INatsClient>(client);
            services.AddMemoryCache();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            
            
            var client = app.ApplicationServices.GetService<INatsClient>();
            var cache = app.ApplicationServices.GetService<IMemoryCache>();
            int numberOfRequestsProcessed = 0;
            await client.SubWithHandlerAsync("mensaje-emitido", msg =>
            {
                //Set the number of requests processed
                cache.Set("mensaje-emitido", ++numberOfRequestsProcessed);

                //simulate a workload
                Thread.Sleep((int)TimeSpan.FromSeconds(2).TotalMilliseconds);

                //Reponse
                client.Pub(msg.ReplyTo, "PONG_MESSAGE");
            });
        }
    }
}
