using Confluent.Kafka;
using static Common.Constants;

/// Allows for demonstration of how consumers are treated differently
/// when they are part of the same group, versus when they have different group IDs
/// - If consumers have the same group ID, messages should be allocated between them
/// - If consumers have different group IDs, they should receive all messages in a broadcast-fashion
Console.WriteLine("Enter consumer group ID:");
var groupId = Console.ReadLine();
if (groupId == null || groupId == string.Empty)
{
    throw new ArgumentException("Group ID cannot be empty");
}

var config = new ConsumerConfig()
{
    GroupId = groupId,
    BootstrapServers = Configuration.KafkaConnectUri,
    AutoOffsetReset = AutoOffsetReset.Earliest,
};

using var consumer = new ConsumerBuilder<Null, string>(config).Build();
using var tokenSource = new CancellationTokenSource();

consumer.Subscribe(Topics.Weather);

Console.WriteLine("Messages will appear below:");
try
{
    while (true)
    {
        var response = consumer.Consume(tokenSource.Token);
        if(response.Message != null)
        {
            Console.WriteLine(response.Message.Value);
        }
    }
}
catch (ConsumeException ex)
{
    Console.WriteLine($"Exception consuming message {ex.Message}");
    throw;
}