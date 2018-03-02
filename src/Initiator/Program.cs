using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Initiator
{
    class Program
    {
        private static IConfigurationRoot Config => GetConfig();

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.Read();
        }

        private static async Task MainAsync(string[] args)
        {
            var settings = new Common.Settings(connectionString: Config["ServiceBus_ConnectionString"], queueName: Config["ServiceBus_QueueName"]);
            var message = new Common.Message { Text = "PING_MESSAGE" };
            ISender<Common.Message> sender = new Sender<Common.Message>(settings);
            for (int i = 0; i < 10; i++)
            {
                await sender.SendAsync(message);
                Console.WriteLine($"Sent {i}");
            }
        }


        private static IConfigurationRoot GetConfig()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
