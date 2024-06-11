using SqlSugar;

namespace GalaFamilyLibrary.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        SqlSugarScope DbClient { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
