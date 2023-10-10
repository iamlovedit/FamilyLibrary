using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using GalaFamilyLibrary.EventBus.Events;
using SqlSugar;

namespace GalaFamilyLibrary.EventBus.IntegrationEventLog
{
    public class IntegrationEventLogEntry
    {
        private static readonly JsonSerializerOptions _indentedOptions = new() { WriteIndented = true };

        private static readonly JsonSerializerOptions _caseInsensitiveOptions =
            new() { PropertyNameCaseInsensitive = true };

        public IntegrationEventLogEntry()
        {
        }

        public IntegrationEventLogEntry(IntegrationEvent integrationEvent, long transactionId)
        {
            EventId = integrationEvent.Id;
            CreationTime = integrationEvent.CreationDate;
            EventTypeName = integrationEvent.GetType().FullName;
            Content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType(), _indentedOptions);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId;
        }

        public long EventId { get; set; }

        public string EventTypeName { get; private set; }

        [SugarColumn(IsIgnore = true)] public string EventTypeShortName => EventTypeName.Split('.')?.Last();

        [SugarColumn(IsIgnore = true)] public IntegrationEvent IntegrationEvent { get; private set; }

        public EventStateEnum State { get; set; }

        public int TimesSent { get; set; }

        public DateTime CreationTime { get; private set; }

        public string Content { get; private set; }

        public long TransactionId { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonSerializer.Deserialize(Content, type, _caseInsensitiveOptions) as IntegrationEvent;
            return this;
        }
    }
}