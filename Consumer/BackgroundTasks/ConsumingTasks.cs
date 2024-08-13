using CashTransMainService.Common;
using CashTransMainService.Settings;
using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;

namespace CashTransMainService.BackgroundTasks
{
    public class ConsumingTasks : IConsumingTask<string, string>
    {
        private int _currentHandlerId;
        private readonly int _preparationHandlerCount;
        private const int InitialHandlerId = 1;
        private readonly ILogger<ConsumingTasks> _logger;
        public ConsumingTasks(ILogger<ConsumingTasks> logger, AppSetting appSetting)
        {
            _logger = logger;
            _currentHandlerId = InitialHandlerId;
        }

        public void Execute(ConsumeResult<string, string> result)
        {
            Console.WriteLine(result.Message);
        }
    }
}
