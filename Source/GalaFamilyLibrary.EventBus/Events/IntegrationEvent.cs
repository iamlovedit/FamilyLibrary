using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = SnowFlakeSingle.instance.NextId();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(long id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty] public long Id { get; private set; }

        [JsonProperty] public DateTime CreationDate { get; private set; }
    }
}