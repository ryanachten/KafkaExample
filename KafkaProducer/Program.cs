using Confluent.Kafka;
using static Common.Constants;

var config = new ProducerConfig
{
    BootstrapServers = Configuration.KafkaConnectUri,
};

using var producer = new ProducerBuilder<Null, string>(config).Build();

Console.WriteLine("Type something to write a message:");
try
{
    string? value;
    while ((value = Console.ReadLine()) != null)
    {

        _ = await producer.ProduceAsync(Topics.Weather, new Message<Null, string>()
        {
            Value = value
        });
    }

}
catch (ProduceException<Null, string> ex)
{
    Console.WriteLine($"Exception producing message {ex.Message}");
	throw;
}