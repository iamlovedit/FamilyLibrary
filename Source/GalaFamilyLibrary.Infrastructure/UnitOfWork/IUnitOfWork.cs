using SqlSugar;

namespace GalaFamilyLibrary.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        SqlSugarScope DbClient { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
