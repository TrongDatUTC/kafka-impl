using CashTransMainService.Common;
using CashTransMainService.Settings;
using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using System.Collections;
using System.Text;

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
            if (result.Message.Headers.Any())
            {
                var header = result.Message.Headers[0].GetValueBytes();
                Console.WriteLine(Encoding.UTF8.GetString(header));
            }

            //Process message right here
        }
    }
}
