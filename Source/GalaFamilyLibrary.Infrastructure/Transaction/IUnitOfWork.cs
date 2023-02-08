using SqlSugar;

namespace GalaFamilyLibrary.Infrastructure.Transaction
{
    public interface IUnitOfWork
    {
        SqlSugarScope DbClient { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
