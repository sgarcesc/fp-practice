using System;

namespace Common
{
    public class Settings
    {
        public Settings(string connectionString, string queueName)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException(nameof(queueName));

            ConnectionString = connectionString;
            QueueName = queueName;
        }

        public string ConnectionString { get; private set; }
        public string QueueName { get; private set; }
    }
}
