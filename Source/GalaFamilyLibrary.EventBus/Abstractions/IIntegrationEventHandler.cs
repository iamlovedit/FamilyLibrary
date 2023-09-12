using GalaFamilyLibrary.EventBus.Events;

namespace GalaFamilyLibrary.EventBus.Abstractions
{
    public interface IIntegrationEventHandler
    {

    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent integrationEvent);
    }
}
