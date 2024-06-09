using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Infrastructure.Seed;

public class DatabaseSeed(AppDbContext appDbContext)
{
    public void InitTablesByClass(Type model)
    {
        if (appDbContext.DbType == DbType.Oracle)
        {
            throw new InvalidOperationException("暂不支持Oracle数据库");
        }
        else
        {
            appDbContext.Database.DbMaintenance.CreateDatabase();
        }
        try
        {
            var types = model.Assembly.DefinedTypes.
             Where(ti => ti.Namespace == model.Namespace && ti.IsClass && ti.GetCustomAttribute<SugarTable>() != null).
             Select(ti => ti.AsType());

            foreach (var type in types)
            {
                if (appDbContext.Database.DbMaintenance.IsAnyTable(type.Name))
                {
                    continue;
                }
                var tableName = type.GetCustomAttribute<SugarTable>()?.TableName ?? type.Name;
                Console.WriteLine($"正在创建表 {tableName}");
                appDbContext.Database.CodeFirst.InitTables(type);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

    }

    public void InitTables(Type program)
    {
        if (appDbContext.DbType == DbType.Oracle)
        {
            throw new InvalidOperationException("暂不支持Oracle数据库");
        }
        else
        {
            appDbContext.Database.DbMaintenance.CreateDatabase();
        }

        try
        {
            var modelTypes = program.Assembly.DefinedTypes.Select(tInfo => tInfo.AsType())
                .Where(t => t.IsClass && t.GetCustomAttribute<SugarTable>() != null);
            foreach (var type in modelTypes)
            {
                if (appDbContext.Database.DbMaintenance.IsAnyTable(type.Name))
                {
                    continue;
                }

                var tableName = type.GetCustomAttribute<SugarTable>()?.TableName ?? type.Name;
                Console.WriteLine($"正在创建表 {tableName}");
                appDbContext.Database.CodeFirst.InitTables(type);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async void InitSeed<T>(string seedFile) where T : class, new()
    {
        var setting = new JsonSerializerSettings();
        JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
        {
            setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            setting.NullValueHandling = NullValueHandling.Ignore;
            return setting;
        });
        if (string.IsNullOrEmpty(seedFile))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(seedFile));
        }

        try
        {
            if (await appDbContext.Database.Queryable<T>().AnyAsync())
            {
                return;
            }

            var json = await File.ReadAllTextAsync(seedFile, Encoding.UTF8);
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            var data = JsonConvert.DeserializeObject<List<T>>(json);
            await appDbContext.GetEntityDB<T>().InsertRangeAsync(data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

}