using GalaFamilyLibrary.EventBus.Events;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.EventBus.IntegrationEventLog.Services
{
    public interface IIntegrationEventLogService:IServiceBase<IntegrationEventLogEntry>
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(long transactionId);
        Task SaveEventAsync(IntegrationEvent integrationEvent);
        
        Task MarkEventAsPublishedAsync(long eventId);
        
        Task MarkEventAsInProgressAsync(long eventId);
        
        Task MarkEventAsFailedAsync(long eventId);
        
    }

    public class IntegrationEventLogService : ServiceBase<IntegrationEventLogEntry>, IIntegrationEventLogService
    {
        public IntegrationEventLogService(IRepositoryBase<IntegrationEventLogEntry> dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(long transactionId)
        {
            throw new NotImplementedException();
        }

        public Task SaveEventAsync(IntegrationEvent integrationEvent)
        {
            throw new NotImplementedException();
        }

        public Task MarkEventAsPublishedAsync(long eventId)
        {
            throw new NotImplementedException();
        }

        public Task MarkEventAsInProgressAsync(long eventId)
        {
            throw new NotImplementedException();
        }

        public Task MarkEventAsFailedAsync(long eventId)
        {
            throw new NotImplementedException();
        }
    }
}