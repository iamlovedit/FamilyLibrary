namespace GalaFamilyLibrary.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private int _count;

    private readonly SqlSugarScope? _dbClient;

    public SqlSugarScope DbClient => _dbClient!;

    public UnitOfWork(ISqlSugarClient sqlSugarClient)
    {
        if (sqlSugarClient is SqlSugarScope scope)
        {
            _dbClient = scope;
        }
    }

    public void BeginTransaction()
    {
        lock (this)
        {
            _count++;
            _dbClient?.BeginTran();
        }
    }

    public void CommitTransaction()
    {
        lock (this)
        {
            _count--;
            if (_count != 0)
            {
                return;
            }

            try
            {
                _dbClient?.CommitTran();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dbClient?.RollbackTran();
            }
        }
    }

    public void RollbackTransaction()
    {
        lock (this)
        {
            _count--;
            _dbClient?.RollbackTran();
        }
    }
}