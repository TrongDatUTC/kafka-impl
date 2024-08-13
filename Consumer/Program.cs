using CashTransMainService.Common;
using CashTransMainService.Extensions;
using CashTransMainService.Settings;
using Manonero.MessageBus.Kafka.Extensions;
using CashTransMainService.BackgroundTasks;

var builder = WebApplication.CreateBuilder(args);
var appSetting = AppSetting.MapValue(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(appSetting);
builder.Services.AddKafkaConsumers(builder =>
{
    builder.AddConsumer<ConsumingTasks>(appSetting.GetConsumerSetting(Constants.ConsumerID));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
app.UseSwagger();
}

app.MapControllers();

app.UseKafkaMessageBus();

app.RunConsumer();

app.RunApplication();
