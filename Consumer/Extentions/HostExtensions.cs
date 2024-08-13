using Manonero.MessageBus.Kafka.Abstractions;
using System.Text.Json.Serialization;
using System.Text.Json;
using CashTransMainService.Common;
using CashTransMainService.Settings;
using Confluent.Kafka;

namespace CashTransMainService.Extensions;

public static class HostExtensions
{
    public static IHost RunConsumer(this IHost host)
    {
        var services = host.Services;
        var appSetting = services.GetRequiredService<AppSetting>();

        var offsetSpecified = 0;

        RunConsumerWithSpecifiedOffset(services, appSetting, offsetSpecified);
        return host;
    }

    private static void RunConsumerWithSpecifiedOffset(IServiceProvider services, AppSetting appSetting, long offsetSpecified)
    {
        var consumerManager = services.GetRequiredService<IKafkaConsumerManager>();
        var consumerSetting = appSetting.GetConsumerSetting(Constants.ConsumerID);
        if(offsetSpecified > 0)
        {
            consumerSetting.SetPartitionsAssignedHandler<string, string>((consumer, topicPartitions) =>
            {
                foreach (var tp in topicPartitions)
                {
                    consumer.Commit(new List<TopicPartitionOffset> {
                        new TopicPartitionOffset(tp, offsetSpecified + 1) //Add one unit to lastest offset because that offset is stored in DB, if read it again, it will be duplicate
                    });
                }
            });
        }
        consumerManager.RunConsumer(Constants.ConsumerID);
    }

    public static void RunApplication(this IHost app)
    {
        var appSetting = app.Services.GetRequiredService<AppSetting>();
        var logger = app.Services.GetRequiredService<ILogger<AppSetting>>();
        var serializeOption = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            WriteIndented = true
        };
        logger.LogInformation($"App started with configuration:\n{JsonSerializer.Serialize(appSetting, serializeOption)}");
        app.Run();
    }
}
