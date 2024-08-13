using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Extensions;
using Producer.Common;
using TestProducer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appSetting = AppSetting.MapValue(builder.Configuration);
var producerConfig = new ProducerConfig
{
    BootstrapServers = appSetting.BootstrapServers,
};
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(appSetting);
builder.Services.AddSingleton<IProducer<string, string>>(new ProducerBuilder<string, string>(producerConfig).Build());
builder.Services.AddKafkaProducers(producerBuilder =>
{
    producerBuilder.AddProducer(appSetting.GetProducerSetting(Constants.ProducerSettingId));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
