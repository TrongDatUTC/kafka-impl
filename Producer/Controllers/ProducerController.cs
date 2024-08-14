using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Producer.Common;
using Producer.Entities;

namespace TestProducer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly IProducer<string, string> _kafkaProducer;
    private readonly AppSetting _appSetting;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProducerController(IProducer<string, string> kafkaProducer, AppSetting appSetting)
    {
        _kafkaProducer = kafkaProducer;
        _appSetting = appSetting;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    [HttpGet("Produce")]
    public async Task<IActionResult> Produce()
    {
        var msg = new KafkaMessage { Id = Guid.NewGuid(), Name = "Nguyen Trong Dat" };
        var kafkaMessageJson = JsonSerializer.Serialize(msg, _jsonOptions);
        var kafkaMessage = new Message<string, string>
        {
            Value = kafkaMessageJson
        };
        try
        {
            var idRequest = HttpContext.Connection.RemoteIpAddress.ToString();
            var messageHeaders = new Headers();
            messageHeaders.Add(new Confluent.Kafka.Header("Partition", Encoding.UTF8.GetBytes(idRequest)));
            kafkaMessage.Headers = messageHeaders;  
            kafkaMessage.Key = idRequest;
            var producerSetting = _appSetting.GetProducerSetting(Constants.ProducerSettingId);
            await _kafkaProducer.ProduceAsync(producerSetting.Topic, kafkaMessage);
            return Ok($"{kafkaMessage.Value}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}