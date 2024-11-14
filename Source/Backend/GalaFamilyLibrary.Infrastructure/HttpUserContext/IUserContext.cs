using System.Security.Claims;
using GalaFamilyLibrary.Infrastructure.Security;

namespace GalaFamilyLibrary.Infrastructure.HttpUserContext;

/// <summary>
/// 用户上下文
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IUserContext<TId> where TId : IEquatable<TId>
{
    /// <summary>
    /// 用户primary key的类型
    /// </summary>
    TId Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    string Username { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// 角色id
    /// </summary>
    string[] RoleIds { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    string[] RoleNames { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    string[] Permissions { get; set; }

    /// <summary>
    /// 访问Ip
    /// </summary>
    string RemoteIpAddress { get; set; }

    /// <summary>
    /// 生成Jwt token信息
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="duration"></param>
    /// <param name="schemeName"></param>
    /// <returns></returns>
    JwtTokenInfo GenerateTokenInfo(IList<Claim>? claims = null,
        int duration = 0,
        string schemeName = JwtBearerDefaults.AuthenticationScheme);

    /// <summary>
    /// 从当前用户上下文获取claims
    /// </summary>
    /// <returns></returns>
    IList<Claim>? GetClaimsFromUserContext(bool includePermissions = false);
}