using Confluent.Kafka;
using static Common.Constants;

var config = new ConsumerConfig()
{
    GroupId = "weather-consumer",
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