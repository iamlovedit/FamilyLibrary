using GalaFamilyLibrary.EventBus.Events;

namespace GalaFamilyLibrary.EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent integrationEvent);

        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;

        void Unsubscribe<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent;
    }
}

