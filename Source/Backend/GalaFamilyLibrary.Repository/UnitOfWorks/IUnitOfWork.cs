using Microsoft.Extensions.Logging;
using SqlSugar;

namespace GalaFamilyLibrary.Repository.UnitOfWorks
{
    public interface IUnitOfWork
    {
        SqlSugarScope DbClient { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }

    public class UnitWork : IDisposable
    {
        public ILogger Logger { get; set; }

        public ISqlSugarClient Database { get; internal set; }

        public ITenant Tenant { get; internal set; }

        public bool HasStarted { get; set; }

        public bool HasCommited { get; set; }

        public bool HasClosed { get; set; }

        public void Dispose()
        {
            if (HasStarted && !HasClosed)
            {
                Logger.LogDebug("UnitOfWork RollbackTran");
                Tenant.RollbackTran();
            }

            if (Database.Ado.Transaction != null || HasClosed)
            {
                return;
            }

            Database.Close();
        }

        public bool Commit()
        {
            if (HasStarted && !HasCommited)
            {
                Logger.LogDebug("UnitOfWork CommitTran");
                Tenant.CommitTran();
                HasCommited = true;
            }

            if (Database.Ado.Transaction == null && !HasClosed)
            {
                Database.Close();
                HasCommited = true;
            }

            return HasCommited;
        }
    }
}