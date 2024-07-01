using Microsoft.Extensions.Logging;
using SqlSugar;

namespace GalaFamilyLibrary.Repository.UnitOfWorks
{
    public class UnitOfWorkManager(ILogger<UnitOfWorkManager> logger, ISqlSugarClient sqlSugarClient)
        : IUnitOfWorkManager
    {
        public SqlSugarScope GetDbClient()
        {
            return sqlSugarClient as SqlSugarScope;
        }

        private int _transactionCount { get; set; }

        public int TransactionCount
        {
            get => _transactionCount;
        }

        public UnitWork CreateUnitWork()
        {
            var unitwork = new UnitWork()
            {
                Logger = logger,
                Database = sqlSugarClient,
                Tenant = (ITenant)sqlSugarClient,
                HasStarted = true
            };
            unitwork.Database.Open();
            unitwork.Tenant.BeginTran();
            logger.LogDebug("UnitOfWork Begin");
            return unitwork;
        }

        public void StartTransaction()
        {
            lock (this)
            {
                _transactionCount++;
                GetDbClient().BeginTran();
            }
        }

        public void CommitTransaction()
        {
            lock (this)
            {
                _transactionCount--;
                if (_transactionCount != 0)
                {
                    return;
                }

                try
                {
                    GetDbClient().CommitTran();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    GetDbClient().RollbackTran();
                }
            }
        }

        public void RollbackTransaction()
        {
            lock (this)
            {
                _transactionCount--;
                GetDbClient().RollbackTran();
            }
        }
    }
}