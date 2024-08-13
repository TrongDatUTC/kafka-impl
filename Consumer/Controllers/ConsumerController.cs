using Microsoft.AspNetCore.Mvc;
using CashTransMainService.Settings;
using CashTransMainService.Common;
using Manonero.MessageBus.Kafka.Abstractions;

namespace CashTransMainService.Controllers
{

    [Route("api/v1/")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly AppSetting _appsetting;
        private readonly IKafkaConsumerManager _kafkaConsumerManager;

        public ConsumerController(AppSetting appsetting, ILogger<ConsumerController> logger, IKafkaConsumerManager kafkaConsumerManager)
        {
            _appsetting           = appsetting;
            _logger               = logger;
            _kafkaConsumerManager = kafkaConsumerManager;
        }

        /// <summary>
        /// Tổng hợp lại cuối ngày.
        /// </summary>
        [HttpPost("start-cash-consumer")]
        public IActionResult StartCashConsumer()
        {
            try
            {
                //start loop consume
                _kafkaConsumerManager.RunConsumer(Constants.ConsumerID);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("stop-cash-consumer")]
        public IActionResult StopCashConsumer()
        {
            try
            {
                // stop loop consume
                _kafkaConsumerManager.StopConsumer(Constants.ConsumerID);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
