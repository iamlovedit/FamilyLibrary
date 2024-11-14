namespace GalaFamilyLibrary.Infrastructure.Repository;

/// <summary>
/// 工作单元
/// </summary>
public interface IUnitOfWork
{
    SqlSugarScope DbClient { get; }

    void BeginTransaction();

    void CommitTransaction();

    void RollbackTransaction();
}