using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

            //Configure the NATS client connection
            var cnInfo = new ConnectionInfo(new Host("nats.cloudapp.net"));
            var client = new NatsClient("request-response", cnInfo);
            client.Connect();

            await client.SubWithHandlerAsync("mensaje-emitido", msg =>
            {
                Thread.Sleep((int)TimeSpan.FromSeconds(2).TotalMilliseconds);
                client.Pub(msg.ReplyTo, "PONG_MESSAGE");
            });
        }
    }
}
