# Kafka Example

# Getting started

- Run `docker-compose up -d` to spin up the Confluent Kafka stack
- Open http://localhost:9021 to view the Confluent Kakfa control centre
- Run producer via `dotnet watch run --project .\KafkaProducer\KafkaProducer.csproj`
- Run consumer via `dotnet watch run --project .\KafkaConsumer\KafkaConsumer.csproj`
