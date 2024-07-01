using SqlSugar;

namespace GalaFamilyLibrary.Repository.UnitOfWorks
{
    public interface IUnitOfWorkManager
    {
        SqlSugarScope GetDbClient();

        int TransactionCount { get; }

        UnitWork CreateUnitWork();

        void StartTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}