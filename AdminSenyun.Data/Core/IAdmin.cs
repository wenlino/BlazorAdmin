namespace AdminSenyun.Data.Core;

public interface IAdmin
{
    /// <summary>
    /// 用户名称
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// 用户显示名称
    /// </summary>
    string? DisplayName { get; }

    IUser UsersService { get; }

    /// <summary>
    /// 通过用户名获取角色列表
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    List<string> GetRoles(string userName);

    /// <summary>
    /// 通过用户获取 Url 授权
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="url">请求地址</param>
    /// <returns></returns>
    Task<bool> AuthorizingNavigation(string userName, string url);

    /// <summary>
    /// 通过用户获取 Block 授权
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="url">请求地址</param>
    /// <param name="blockName">Block 名称</param>
    /// <returns></returns>
    bool AuthorizingBlock(string userName, string url, string blockName);

    /// <summary>
    /// 获取Block全新
    /// </summary>
    /// <param name="blockName"></param>
    /// <returns></returns>
    bool IsBlock(string blockName);

    /// <summary>
    /// 检查页面内模块权限 异步
    /// </summary>
    /// <param name="blockName"></param>
    /// <returns></returns>
    Task<bool> IsBlockAsync(string blockName);

    /// <summary>
    /// 检查是否有菜单地址访问全新啊
    /// </summary>
    /// <returns></returns>
    bool IsNavigation();

    /// <summary>
    /// 检查是否有菜单地址访问全新啊 异步
    /// </summary>
    /// <returns></returns>
    Task<bool> IsNavigationAsync();
}
