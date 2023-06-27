using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class SqlsugarSetup
{
    public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        void ConfigAction(SqlSugarClient client)
        {
#if DEBUG
            client.Aop.OnLogExecuting = (sql, paras) => { Console.WriteLine(sql); };
#endif
        }
        var sqlsugar = new SqlSugarScope(new ConnectionConfig()
        {
            DbType = DbType.PostgreSQL,
            IsAutoCloseConnection = true,
            ConnectionString = configuration["DATABASE_CONNECTION_STRING"],
            InitKeyType = InitKeyType.Attribute,
            MoreSettings=new ConnMoreSettings
            {
                PgSqlIsAutoToLower=false,
                PgSqlIsAutoToLowerCodeFirst=false
            }
        }, ConfigAction);
        sqlsugar.QueryFilter.AddTableFilter<IDeletable>(d => !d.IsDeleted);
        services.AddSingleton<ISqlSugarClient>(sqlsugar);
    }
}