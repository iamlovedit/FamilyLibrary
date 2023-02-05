using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Infrastructure.Seed;

public class DbSeed
{
    public static void InitTables(AppDbContext appDbContext, string modelsAssembly, string modelNamespace)
    {
        if (appDbContext == null) throw new ArgumentNullException(nameof(appDbContext));
        if (string.IsNullOrEmpty(modelsAssembly))
            throw new ArgumentException("Value cannot be null or empty.", nameof(modelsAssembly));
        if (string.IsNullOrEmpty(modelNamespace))
            throw new ArgumentException("Value cannot be null or empty.", nameof(modelNamespace));

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
            var assembly = Assembly.GetExecutingAssembly();
            var modelTypes = assembly.DefinedTypes.Select(tInfo => tInfo.AsType()).Where(t => t.IsClass && t.GetCustomAttribute<SugarTable>() != null);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            //var referencedAssemblies = Directory.GetFiles(path, modelsAssembly)
            //    .Select(Assembly.LoadFrom).ToArray();
            //var modelTypes = referencedAssemblies
            //    .SelectMany(a => a.DefinedTypes)
            //    .Select(type => type.AsType())
            //    .Where(t => t.IsClass && t.Namespace != null && t.Namespace.Equals(modelNamespace));

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

    public static async void InitSeed<T>(AppDbContext appDbContext, string seedFileName) where T : class, new()
    {
        if (appDbContext == null) throw new ArgumentNullException(nameof(appDbContext));
        if (string.IsNullOrEmpty(seedFileName))
            throw new ArgumentException("Value cannot be null or empty.", nameof(seedFileName));

        if (await appDbContext.Database.Queryable<T>().AnyAsync())
        {
            return;
        }
        var json = await File.ReadAllTextAsync(seedFileName, Encoding.UTF8);
        if (string.IsNullOrEmpty(json))
        {
            return;
        }
        var data = JsonConvert.DeserializeObject<List<T>>(json);
        await appDbContext.GetEntityDB<T>().InsertRangeAsync(data);
    }
}