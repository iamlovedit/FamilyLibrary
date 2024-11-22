using System.Reflection;
using GalaFamilyLibrary.Infrastructure.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json.Serialization;
using ILogger = DnsClient.Internal.ILogger;

namespace GalaFamilyLibrary.Infrastructure.Seed;

public class DatabaseSeed(DatabaseContext databaseContext, ILogger<DatabaseSeed> logger)
{
    public void GenerateTablesByClass<T>() where T : class, new()
    {
        if (databaseContext.DbType == DbType.Oracle)
        {
            throw new InvalidOperationException("暂不支持Oracle数据库");
        }
        else
        {
            databaseContext.Database.DbMaintenance.CreateDatabase();
        }

        try
        {
            var modelType = typeof(T);
            var types = modelType.Assembly.DefinedTypes.Where(ti =>
                    ti.Namespace == modelType.Namespace && ti.IsClass && ti.GetCustomAttribute<SugarTable>() != null)
                .Select(ti => ti.AsType());

            foreach (var type in types)
            {
                var tableName = type.GetCustomAttribute<SugarTable>()?.TableName ?? type.Name;
                Console.WriteLine($"table is initializing: {tableName}");
                databaseContext.Database.CodeFirst.InitTables(type);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }


    public async Task GenerateSeedAsync<T>(string seedFile) where T : class, new()
    {
        if (string.IsNullOrEmpty(seedFile))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(seedFile));
        }

        try
        {
            if (await databaseContext.Database.Queryable<T>().AnyAsync())
            {
                return;
            }

            var json = await File.ReadAllTextAsync(seedFile, Encoding.UTF8);
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            var data = json.Deserialize<List<T>>();
            await databaseContext.GetEntityDatabase<T>().InsertRangeAsync(data);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task GenerateMongoSeedAsync<T>(string seedFile) where T : class, new()
    {
        if (string.IsNullOrEmpty(seedFile))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(seedFile));
        }

        try
        {
            var collection = databaseContext.MongoDatabase.GetCollection<T>(typeof(T).Name);
            if (await collection.AsQueryable().AnyAsync())
            {
                return;
            }

            var json = await File.ReadAllTextAsync(seedFile, Encoding.UTF8);
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            var data = json.Deserialize<List<T>>();
            await collection.InsertManyAsync(data);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }
}