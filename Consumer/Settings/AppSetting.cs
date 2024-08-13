using CashTransMainService.Common;
using Manonero.Logger.Kafka.Settings;
using Manonero.MessageBus.Kafka.Settings;

namespace CashTransMainService.Settings;

public class AppSetting
{
    public string BootstrapServers { get; init; }
    public ConsumerSetting[] ConsumerSettings { get; init; }

    public static AppSetting MapValue(IConfiguration configuration)
    {
        var bootstrapServers   = configuration[nameof(BootstrapServers)];

        var consumerConfigurations = configuration.GetSection(nameof(ConsumerSettings)).GetChildren();
        var consumerSettings = new List<ConsumerSetting>();
        foreach (var consumerConfiguration in consumerConfigurations)
        {
            var consumerSetting = ConsumerSetting.MapValue(consumerConfiguration, bootstrapServers);
            if (!consumerSettings.Contains(consumerSetting))
                consumerSettings.Add(consumerSetting);
        }

        var loggingSetting = configuration.GetSection(nameof(LoggingSetting)).Get<LoggingSetting>();
        if (loggingSetting != null) loggingSetting.BootstrapServers = bootstrapServers;

        var disruptorSettings = new List<DisruptorSetting>();

        var setting = new AppSetting
        {
            BootstrapServers   = bootstrapServers,
            ConsumerSettings   = consumerSettings.ToArray()
        };
        return setting;
    }

    public ConsumerSetting GetConsumerSetting(string id)
        => ConsumerSettings.FirstOrDefault(consumerSetting => consumerSetting.Id.Equals(id));
}
