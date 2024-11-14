using System.Reflection;
using System.Security.Claims;
using Asp.Versioning;
using GalaFamilyLibrary.Infrastructure.Domains;
using GalaFamilyLibrary.Infrastructure.Filters;
using GalaFamilyLibrary.Infrastructure.HttpUserContext;
using GalaFamilyLibrary.Infrastructure.Options;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Repository.Mongo;
using GalaFamilyLibrary.Infrastructure.Security;
using GalaFamilyLibrary.Infrastructure.Seed;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GalaFamilyLibrary.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddDefaultInfrastructure<T>(this WebApplicationBuilder builder)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;
        var configuration = builder.Configuration;
        services.AddDefaultUserContext<T>()
            .AddDefaultAesEncryption()
            .AddDefaultAuthentication(configuration)
            .AddDefaultAuthorize(configuration)
            .AddDefaultControllers()
            .AddDefaultCors(configuration)
            .AddDefaultApiVersioning(configuration)
            .AddDefaultRedis(configuration)
            .AddDefaultSerilog(configuration)
            .AddDefaultRepositoryContext()
            .AddDefaultSqlSugar(configuration, builder.Environment)
            .AddDefaultMongoDb(configuration)
            .AddDefaultTokenContext()
            .AddDefaultDatabaseSeed()
            .AddDefaultSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            })
            .AddMapster();
        builder.Host.UseSerilog(Log.Logger, true);
        return builder;
    }

    public static TOptions? GetOptions<TOptions>(this IConfiguration configuration, string sectionName)
        where TOptions : OptionsBase
    {
        return configuration.GetSection(sectionName).Get<TOptions>();
    }

    /// <summary>
    /// 配置redis连接服务,以及redis数据访问仓储
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultRedis(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        var redisOptions = configuration.GetOptions<RedisOptions>(RedisOptions.SectionName);
        if (redisOptions is null || !redisOptions.IsEnable)
        {
            return services;
        }

        services.TryAddScoped<IRedisBasketRepository, RedisBasketRepository>();
        services.TryAddSingleton<ConnectionMultiplexer>(_ =>
        {
            var host = configuration["REDIS_HOST"] ?? redisOptions.Host;
            ArgumentException.ThrowIfNullOrEmpty(host);
            var password = configuration["REDIS_PASSWORD"] ?? redisOptions.Password;
            ArgumentException.ThrowIfNullOrEmpty(password);
            var serviceName = configuration["REDIS_SERVICE_NAME"] ?? redisOptions.ServiceName;
            var connectionString = string.IsNullOrEmpty(serviceName)
                ? $"{host},password={password}"
                : $"{host},password={password},serviceName={serviceName}";
            var redisConfig = ConfigurationOptions.Parse(connectionString, true);
            redisConfig.ResolveDns = true;
            return ConnectionMultiplexer.Connect(redisConfig);
        });
        return services;
    }


    /// <summary>
    /// 配置认证服务，包含jwt认证
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="builderOptions">自定义认证服务配置</param>
    /// <param name="configureAuthenticationOptions">配置认证选项</param>
    /// <param name="configureJwtBearerOptions">配置jwt认证选项</param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<AuthenticationBuilder>? builderOptions = null,
        Action<AuthenticationOptions>? configureAuthenticationOptions = null,
        Action<JwtBearerOptions>? configureJwtBearerOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        var audienceOptions = configuration.GetOptions<AudienceOptions>(AudienceOptions.SectionName);
        if (audienceOptions is null || !audienceOptions.IsEnable)
        {
            return services;
        }

        services.TryAddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerOptionsPostConfigureOptions>();
        var key = configuration["AUDIENCE_KEY"] ?? audienceOptions.Secret;
        ArgumentException.ThrowIfNullOrEmpty(key);
        var buffer = Encoding.UTF8.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(buffer);

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidIssuer = audienceOptions.Issuer,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = audienceOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(0),
            RequireExpirationTime = true,
            RoleClaimType = ClaimTypes.Role,
            LifetimeValidator = (before, expires, token, parameters) =>
                before < DateTime.UtcNow - parameters.ClockSkew && DateTime.UtcNow < expires + parameters.ClockSkew
        };

        var builder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = nameof(DefaultAuthenticationHandler);
            options.DefaultForbidScheme = nameof(DefaultAuthenticationHandler);
            configureAuthenticationOptions?.Invoke(options);
        });
        builder.AddScheme<AuthenticationSchemeOptions, DefaultAuthenticationHandler>(
            nameof(DefaultAuthenticationHandler),
            options => { });
        builder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
            configureJwtBearerOptions?.Invoke(options);
        });
        builderOptions?.Invoke(builder);
        return services;
    }


    /// <summary>
    /// 添加权限认证，权限Policy、Roles从配置中获取
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configureAuthorizeBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultAuthorize(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<AuthorizationBuilder>? configureAuthorizeBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        var audienceOptions = configuration.GetSection(AudienceOptions.SectionName).Get<AudienceOptions>();
        if (audienceOptions is null || !audienceOptions.IsEnable)
        {
            return services;
        }

        var key = configuration["AUDIENCE_KEY"] ?? audienceOptions.Secret;
        ArgumentException.ThrowIfNullOrEmpty(key);
        var buffer = Encoding.UTF8.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(buffer);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        services.TryAddSingleton(new JwtContext(
            audienceOptions.Issuer,
            audienceOptions.Audience,
            audienceOptions.Duration,
            signingCredentials));

        var builder = services.AddAuthorizationBuilder();
        builder.AddPolicy(audienceOptions.Policy!, policy =>
            policy.RequireRole(audienceOptions.Roles!)
                .Build());
        configureAuthorizeBuilder?.Invoke(builder);
        return services;
    }

    /// <summary>
    /// 配置controller，包含过滤器、json序列化以及模型验证等配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureControllers">配置Controllers</param>
    /// <param name="configureMvcOptions">mvc配置选项</param>
    /// <param name="configureMvcNewtonsoftJsonOptions">json序列化配置选项</param>
    /// <param name="configureApiBehaviorOptions">api行为配置选项</param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultControllers(this IServiceCollection services,
        Action<IMvcBuilder>? configureControllers = null,
        Action<MvcOptions>? configureMvcOptions = null,
        Action<MvcNewtonsoftJsonOptions>? configureMvcNewtonsoftJsonOptions = null,
        Action<ApiBehaviorOptions>? configureApiBehaviorOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        var mvcBuilder = services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionsFilter>();
            options.Filters.Add<IdempotencyFilter>();
            configureMvcOptions?.Invoke(options);
        });

        mvcBuilder.AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            configureMvcNewtonsoftJsonOptions?.Invoke(options);
        });
        mvcBuilder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = _ =>
            {
                var message = new MessageData(false, "the input value not valid", 400);
                return new OkObjectResult(message.Serialize());
            };
            configureApiBehaviorOptions?.Invoke(options);
        });
        configureControllers?.Invoke(mvcBuilder);
        return services;
    }

    /// <summary>
    /// 配置Aes加密对称加密服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultAesEncryption(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddSingleton<IEncryptionService, EncryptionService>();
        return services;
    }

    /// <summary>
    ///  配置mongodb连接以及仓储
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultMongoDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(configuration);

        var mongoDbOptions = configuration.GetOptions<MongoDbOptions>(MongoDbOptions.SectionName);
        if (mongoDbOptions is null || !mongoDbOptions.IsEnable)
        {
            return services;
        }

        var host = configuration[MongoDbOptions.MongoHost] ?? mongoDbOptions.Host;
        ArgumentException.ThrowIfNullOrEmpty(host);

        var user = configuration[MongoDbOptions.MongoUser] ?? mongoDbOptions.User;
        ArgumentException.ThrowIfNullOrEmpty(user);

        var password = configuration[MongoDbOptions.MongoPassword] ?? mongoDbOptions.Password;
        ArgumentException.ThrowIfNullOrEmpty(password);

        var port = configuration[MongoDbOptions.MongoPort] ?? mongoDbOptions.Port;
        ArgumentException.ThrowIfNullOrEmpty(port);

        var database = configuration[MongoDbOptions.MongoDatabase] ?? mongoDbOptions.Database;
        ArgumentException.ThrowIfNullOrEmpty(database);
        var connectionString = $"mongodb://{user}:{password}@{host}:{port}/{database}";
        services.TryAddSingleton<IMongoDatabase>(_ =>
            new MongoClient(connectionString)
                .GetDatabase(mongoDbOptions.Database));
        services.TryAddScoped(typeof(IMongoRepositoryBase<,>), typeof(MongoRepositoryBase<,>));
        return services;
    }

    /// <summary>
    /// 配置数据库仓储服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultRepositoryContext(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddScoped<IUnitOfWork, UnitOfWork>();
        services.TryAddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        return services;
    }

    /// <summary>
    /// 配置sqlsugar ORM连接
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultSqlSugar(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment hostEnvironment,
        List<ConnectionConfig>? connectionConfigs = null,
        Action<object, DataFilterModel>? onDataExecuting = null,
        Action<DiffLogModel>? onDiffLogEvent = null
    )
    {
        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(configuration);

        ArgumentNullException.ThrowIfNull(hostEnvironment);

        var sqlSugarOptions = configuration.GetOptions<SqlSugarOptions>(SqlSugarOptions.SectionName);
        if (sqlSugarOptions is null || !sqlSugarOptions.IsEnable)
        {
            return services;
        }

        if (sqlSugarOptions.SnowFlake?.IsEnable ?? false)
        {
            var workerId = configuration["SNOWFLAKES_WORKERID"]?.ObjToInt() ?? sqlSugarOptions.SnowFlake?.WorkerId;
            ArgumentNullException.ThrowIfNull(workerId);
            SnowFlakeSingle.WorkId = (int)workerId;
        }

        var server = configuration["DB_HOST"] ?? sqlSugarOptions.Server;
        ArgumentException.ThrowIfNullOrEmpty(server);

        var port = configuration["DB_PORT"] ?? sqlSugarOptions.Port.ToString();
        ArgumentException.ThrowIfNullOrEmpty(port);

        var database = configuration["DB_DATABASE"] ?? sqlSugarOptions.Database;
        ArgumentException.ThrowIfNullOrEmpty(database);

        var user = configuration["DB_USER"] ?? sqlSugarOptions.User;
        ArgumentException.ThrowIfNullOrEmpty(user);

        var password = configuration["DB_PASSWORD"] ?? sqlSugarOptions.Password;
        ArgumentException.ThrowIfNullOrEmpty(password);

        var connectionString = $"server={server};port={port};database={database};userid={user};password={password};";
        connectionConfigs ??= [];
        connectionConfigs.Add(new ConnectionConfig()
        {
            DbType = DbType.PostgreSQL,
            ConnectionString = connectionString,
            InitKeyType = InitKeyType.Attribute,
            IsAutoCloseConnection = true,
            MoreSettings = new ConnMoreSettings
            {
                PgSqlIsAutoToLower = false,
                PgSqlIsAutoToLowerCodeFirst = false,
            },
            ConfigureExternalServices = new ConfigureExternalServices()
            {
                EntityService = (p, c) =>
                {
                    if (c.IsPrimarykey == false && new NullabilityInfoContext()
                            .Create(p).WriteState is NullabilityState.Nullable)
                    {
                        c.IsNullable = true;
                    }

                    c.DbColumnName = UtilMethods.ToUnderLine(c.DbColumnName);
                },
                EntityNameService = (p, c) => { c.DbTableName = UtilMethods.ToUnderLine(c.DbTableName); }
            }
        });

        var sugarScope = new SqlSugarScope(connectionConfigs, client =>
        {
            client.QueryFilter.AddTableFilter<IDeletable>(d => !d.IsDeleted);
            if (hostEnvironment.IsDevelopment() || hostEnvironment.IsStaging())
            {
                client.Aop.OnLogExecuted = (sql, parameters) =>
                {
                    var elapsed = client.Ado.SqlExecutionTime.TotalSeconds;
                    Log.Logger.Information("sql: {sql}  elapsed: {time} seconds", sql, elapsed);
                };
            }

            onDataExecuting ??= (o, entityInfo) =>
            {
                if (entityInfo is
                    {
                        OperationType: DataFilterType.InsertByObject, PropertyName: nameof(IDateAbility.CreatedDate)
                    })
                {
                    entityInfo.SetValue(DateTime.Now);
                }

                if (entityInfo is
                    { OperationType: DataFilterType.UpdateByObject, PropertyName: nameof(IDateAbility.UpdatedDate) })
                {
                    entityInfo.SetValue(DateTime.Now);
                }
            };

            client.Aop.DataExecuting = onDataExecuting;
            client.Aop.OnDiffLogEvent = onDiffLogEvent;
        });
        services.AddSingleton<ISqlSugarClient>(sugarScope);
        return services;
    }

    /// <summary>
    ///  配置jwt相关上下文
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultTokenContext(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddSingleton<JsonWebTokenHandler>();
        services.TryAddSingleton<DefaultTokenHandler>();
        return services;
    }

    /// <summary>
    /// 配置用户上下文服务 T为主键类型
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDefaultUserContext<T>(this IServiceCollection services) where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddHttpContextAccessor();
        services.TryAddScoped(typeof(IUserContext<T>), typeof(UserContext<T>));
        return services;
    }

    /// <summary>
    /// 配置跨域
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultCors(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        var corsOptions = configuration.GetOptions<CorsOptions>(CorsOptions.SectionName);
        if (corsOptions is null || !corsOptions.IsEnable)
        {
            return services;
        }

        services.AddCors(options =>
        {
            options.AddPolicy(CorsOptions.SectionName,
                policy =>
                {
                    if (corsOptions.AllowAnyHeader)
                    {
                        policy.AllowAnyHeader();
                    }
                    else
                    {
                        policy.WithHeaders(corsOptions.Headers);
                    }

                    if (corsOptions.AllowAnyMethod)
                    {
                        policy.AllowAnyMethod();
                    }
                    else
                    {
                        policy.WithMethods(corsOptions.Methods);
                    }

                    if (corsOptions.AllowAnyOrigin)
                    {
                        policy.AllowAnyOrigin();
                    }
                    else
                    {
                        policy.WithOrigins(corsOptions.Origins);
                    }
                }
            );
        });
        return services;
    }

    /// <summary>
    /// 配置api版本
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configureApiVersioningBuilder"></param>
    /// <param name="configureApiVersioningOptions"></param>
    /// <param name="configureApiExplorerOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultApiVersioning(this IServiceCollection services,
        IConfiguration configuration,
        Action<IApiVersioningBuilder>? configureApiVersioningBuilder = null,
        Action<ApiVersioningOptions>? configureApiVersioningOptions = null,
        Action<ApiExplorerOptions>? configureApiExplorerOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var versionOptions = configuration.GetOptions<VersionOptions>(VersionOptions.SectionName);
        if (versionOptions is null || !versionOptions.IsEnable)
        {
            return services;
        }

        var builder = services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader(versionOptions!.HeaderName),
                new MediaTypeApiVersionReader(versionOptions!.ParameterName));
            configureApiVersioningOptions?.Invoke(options);
        }).AddApiExplorer(builder =>
        {
            builder.GroupNameFormat = "'v'VVV";
            builder.SubstituteApiVersionInUrl = true;
            configureApiExplorerOptions?.Invoke(builder);
        });
        configureApiVersioningBuilder?.Invoke(builder);
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        return services;
    }

    /// <summary>
    ///  配置Serilog
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultSerilog(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        var serilogOptions = configuration.GetSection(SerilogOptions.Name).Get<SerilogOptions>();
        if (serilogOptions is null || !serilogOptions.IsEnable)
        {
            return services;
        }

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console(theme: AnsiConsoleTheme.Code);

        if (serilogOptions.WriteFile)
        {
            loggerConfiguration =
                loggerConfiguration.WriteTo.File(Path.Combine("logs", "log"), rollingInterval: RollingInterval.Hour);
        }

        if (serilogOptions.SeqOptions?.IsEnable ?? false)
        {
            var serverUrl = configuration["SEQ_URL"] ?? serilogOptions.SeqOptions.Address;
            ArgumentException.ThrowIfNullOrEmpty(serverUrl);
            var apiKey = configuration["SEQ_APIKEY"] ?? serilogOptions.SeqOptions.Secret;
            ArgumentException.ThrowIfNullOrEmpty(apiKey);
            loggerConfiguration = loggerConfiguration.WriteTo.Seq(serverUrl, apiKey: apiKey);
        }

        Log.Logger = loggerConfiguration.CreateLogger();
        services.AddLogging(logBuilder =>
        {
            logBuilder.ClearProviders();
            logBuilder.AddSerilog(Log.Logger);
        });
        return services;
    }

    public static IServiceCollection AddDefaultDatabaseSeed(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddScoped<DatabaseContext>();
        services.TryAddScoped<DatabaseSeed>();
        return services;
    }

    public static IServiceCollection AddDefaultSwaggerGen(this IServiceCollection services,
        Action<SwaggerGenOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
            configureOptions?.Invoke(options);
        });
        return services;
    }
}