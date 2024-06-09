using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.EventBus.Events
{
    [method: JsonConstructor]
    public class IntegrationEvent(long id, DateTime createDate)
    {
        public IntegrationEvent() : this(SnowFlakeSingle.instance.NextId(), DateTime.UtcNow)
        {
        }

        [JsonProperty] public long Id { get; private set; } = id;

        [JsonProperty] public DateTime CreationDate { get; private set; } = createDate;
    }
}