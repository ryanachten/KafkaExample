namespace Common;

public static class Constants
{
    /// <summary>
    /// Common config. Yes it should be defined in config but I cbf
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// URI for the cp-server-connect-datagen
        /// </summary>
        public static readonly string KafkaConnectUri = "localhost:9092";
    }

    /// <summary>
    /// Kafka topics to publish and subscribe to
    /// </summary>
    public static class Topics
    {
        public static readonly string Weather = "weather";
    }
}
