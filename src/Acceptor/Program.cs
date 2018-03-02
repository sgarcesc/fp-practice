using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Acceptor
{
    class Program
    {
        private static IConfigurationRoot Config => GetConfig();

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private static async Task MainAsync(string[] args)
        {
            //Thread.Sleep(TimeSpan.FromSeconds(10));
            var settings = new Common.Settings(connectionString: Config["ServiceBus_ConnectionString"], queueName: Config["ServiceBus_QueueName"]);
            IReceiver<Common.Message> receiver = new Receiver<Common.Message>(settings);
            receiver.Receive(
                message =>
                {
                    Console.WriteLine(message.Text);
                    return MessageProcessResponse.Complete;
                },
                ex => Console.WriteLine(ex.Message),
                () => Console.WriteLine("Waiting..."));
            await Task.CompletedTask;
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
