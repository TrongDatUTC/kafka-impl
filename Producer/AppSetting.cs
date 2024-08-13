using Manonero.MessageBus.Kafka.Settings;

namespace TestProducer;

public class AppSetting
{
    public string BootstrapServers { get; init; }
    public ProducerSetting[] ProducerSettings { get; init; }

    public static AppSetting MapValue(IConfiguration configuration)
    {
        var bootstrapServers = configuration[nameof(BootstrapServers)];
        var producerConfigurations = configuration.GetSection(nameof(ProducerSettings)).GetChildren();
        var producerSettings = new List<ProducerSetting>();

        foreach (var producerConfiguration in producerConfigurations)
        {
            var producerSetting = ProducerSetting.MapValue(producerConfiguration, bootstrapServers);
            if (!producerSettings.Contains(producerSetting))
                producerSettings.Add(producerSetting);
        }

        var setting = new AppSetting
        {
            BootstrapServers = bootstrapServers,
            ProducerSettings = producerSettings.ToArray()
        };
        return setting;
    }

    public ProducerSetting GetProducerSetting(string id)
        => ProducerSettings.FirstOrDefault(producerSetting => producerSetting.Id.Equals(id));
}
