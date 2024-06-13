using GalaFamilyLibrary.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class SqlsugarSetup
{
    public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(configuration);

        SnowFlakeSingle.WorkId = configuration["SNOWFLAKES_WORKID"]?.ObjToInt() ?? throw new ArgumentNullException("Snowflakes workid is null");
        
        var connectionString = $"server={configuration["DATABASE_HOST"]};" +
                               $"port={configuration["DATABASE_PORT"]};" +
                               $"database={configuration["DATABASE_DATABASE"]};" +
                               $"userid={configuration["DATABASE_USERID"]};" +
                               $"password={configuration["DATABASE_PASSWORD"]};";

       
        var connectionConfig = new ConnectionConfig()
        {
            DbType = DbType.PostgreSQL,
            ConnectionString = connectionString,
            InitKeyType = InitKeyType.Attribute,
            IsAutoCloseConnection = true,
            MoreSettings = new ConnMoreSettings()
            {
                PgSqlIsAutoToLower = false,
                PgSqlIsAutoToLowerCodeFirst = false,
            }
        };
        
        var sugarScope = new SqlSugarScope(connectionConfig, config =>
        {
            config.QueryFilter.AddTableFilter<IDeletable>(d => !d.IsDeleted);
            if (hostEnvironment.IsDevelopment() || hostEnvironment.IsStaging())
            {
                config.Aop.OnLogExecuting = (sql, parameters) => { Log.Logger.Information(sql); };
            }
        });
        services.AddSingleton<ISqlSugarClient>(sugarScope);
    }
}