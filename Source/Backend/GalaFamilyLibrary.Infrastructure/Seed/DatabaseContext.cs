using MongoDB.Driver;

namespace GalaFamilyLibrary.Infrastructure.Seed;

public class DatabaseContext
{
    public DatabaseContext(ISqlSugarClient sqlSugarClient, IMongoDatabase mongoDatabase)
    {
        MongoDatabase = mongoDatabase;
        if (sqlSugarClient is SqlSugarScope scope)
        {
            Database = scope;
        }
    }

    public IMongoDatabase MongoDatabase { get; }

    public SqlSugarScope Database { get; private set; }

    public DbType DbType { get; set; }

    public SimpleClient<T> GetEntityDatabase<T>() where T : class, new()
    {
        return new SimpleClient<T>(Database);
    }
}