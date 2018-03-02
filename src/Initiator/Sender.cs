using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Initiator
{
    internal class Sender<T> : ISender<T> where T : class
    {
        private readonly Common.Settings _settings;
        private readonly QueueClient _client;

        public Sender(Common.Settings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _client = new QueueClient(_settings.ConnectionString, _settings.QueueName);
        }

        public Task SendAsync(T item)
        {
            return SendAsync(item, null);
        }

        public Task SendAsync(T item, Dictionary<string, object> properties)
        {
            var json = JsonConvert.SerializeObject(item);
            var message = new Message(Encoding.UTF8.GetBytes(json));

            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    message.UserProperties.Add(prop.Key, prop.Value);
                }
            }

            return _client.SendAsync(message);
        }
    }
}
