using GalaFamilyLibrary.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class SqlsugarSetup
{
    public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(webHostEnvironment);

        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(configuration);

        var workId = configuration.GetSection("SnowFlake")["WorkId"].ObjToInt();
        SnowFlakeSingle.WorkId = workId;
        var connectionString = $"server={configuration["DATABASE_HOST"]};" +
                               $"port={configuration["DATABASE_PORT"]};" +
                               $"database={configuration["DATABASE_DATABASE"]};" +
                               $"userid={configuration["DATABASE_USERID"]};" +
                               $"password={configuration["DATABASE_PASSWORD"]};";

        void ConfigAction(SqlSugarClient client)
        {
            client.QueryFilter.AddTableFilter<IDeletable>(d => !d.IsDeleted);
            if (webHostEnvironment.IsDevelopment() || webHostEnvironment.IsStaging())
            {
                client.Aop.OnLogExecuting = (sql, paras) => { Console.WriteLine(sql); };
            }
        }

        var sqlsugar = new SqlSugarScope(new ConnectionConfig()
        {
            DbType = DbType.PostgreSQL,
            IsAutoCloseConnection = true,
            ConnectionString = connectionString,
            InitKeyType = InitKeyType.Attribute,
            MoreSettings = new ConnMoreSettings
            {
                PgSqlIsAutoToLower = false,
                PgSqlIsAutoToLowerCodeFirst = false
            }
        }, ConfigAction);
        services.AddSingleton<ISqlSugarClient>(sqlsugar);
    }
}