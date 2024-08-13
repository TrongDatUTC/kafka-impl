namespace CashTransMainService.Settings
{
    public class DisruptorSetting
    {
        public string Id { get; init; }
        public int RingInBufferSize { get; init; }
        public int RingOutBufferSize { get; init; }
        public int PreparationHandlerCount { get; init; }
        public int PublishingBatchSize { get; init; }
        public static DisruptorSetting MapValue(IConfiguration configuration)
        {
            int.TryParse(configuration[nameof(RingInBufferSize)], out int ringInBufferSize);
            int.TryParse(configuration[nameof(RingOutBufferSize)], out int ringOutBufferSize);
            int.TryParse(configuration[nameof(PreparationHandlerCount)], out int preparationHandlerCount);
            int.TryParse(configuration[nameof(PublishingBatchSize)], out int publishingBatchSize);
            var id = configuration[nameof(Id)];
            return new DisruptorSetting
            {
                Id = id,
                RingInBufferSize = ringInBufferSize,
                RingOutBufferSize = ringOutBufferSize,
                PreparationHandlerCount = preparationHandlerCount,
                PublishingBatchSize = publishingBatchSize
            };

        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(DisruptorSetting))
            {
                return false;
            }
            DisruptorSetting setting = (DisruptorSetting)obj;
            return setting.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
