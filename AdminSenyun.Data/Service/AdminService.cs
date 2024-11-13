using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Security.Principal;

namespace AdminSenyun.Data.Service;

/// <summary>
/// 权限管理模块
/// </summary>
/// <param name="user"></param>
/// <param name="navigations"></param>
/// <param name="navigationManager"></param>
/// <param name="identityService"></param>
public class AdminService(IUser user, INavigation navigations, NavigationManager navigationManager, IdentityService identityService) : IAdmin
{
    private string? userName;
    public string? UserName => userName ??= identityService.UserName;


    private string? displayName;
    public string? DisplayName => displayName ??= user.GetUserByUserName(UserName)?.DisplayName ?? "未注册账户";


    public IUser UsersService => user;

    /// <summary>
    /// 通过用户名获取角色集合方法
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetRoles(string userName) => user.GetRoles(userName);


    /// <summary>
    /// 通过用户名检查当前请求 Url 是否已授权方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<bool> AuthorizingNavigation(string userName, string url)
    {
        var ret = navigations.GetAllMenus(userName)
            .Any(m => m.Url?.Contains(url, StringComparison.OrdinalIgnoreCase) ?? false);
        return Task.FromResult(ret);
    }

    /// <summary>
    /// 通过用户名检查当前请求 Url 是否已授权方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="url"></param>
    /// <param name="blockName"></param>
    /// <returns></returns>
    public bool AuthorizingBlock(string userName, string url, string blockName)
    {
        var ret = user.GetRoles(userName).Any(i => i.Equals("Administrators", StringComparison.OrdinalIgnoreCase));
        if (!ret)
        {
            var menus = navigations.GetAllMenus(userName);
            var menu = menus.FirstOrDefault(m => m.Url?.Contains(url, StringComparison.OrdinalIgnoreCase) ?? false);
            if (menu != null)
            {
                ret = menus.Any(m => m.ParentId == menu.Id && m.IsResource == EnumResource.Block && m.Url.Equals(blockName, StringComparison.OrdinalIgnoreCase));
            }
        }
        return ret;
    }


    public bool IsBlock(string blockName)
    {
        var userName = identityService.UserName;
        var url = navigationManager.ToBaseRelativePath(navigationManager.Uri);

        if (string.IsNullOrWhiteSpace(userName))
            return false;

        return AuthorizingBlock(userName, url, blockName);
    }

    public Task<bool> IsBlockAsync(string blockName)
    {
        return Task.FromResult(IsBlock(blockName));
    }

    public bool IsNavigation()
    {
        var userName = identityService.UserName;
        var url = navigationManager.ToBaseRelativePath(navigationManager.Uri);

        var ret = navigations.GetAllMenus(userName).Any(m => m.Url?.Contains(url, StringComparison.OrdinalIgnoreCase) ?? false);
        return ret;
    }

    public Task<bool> IsNavigationAsync()
    {
        return Task.FromResult(IsNavigation());
    }
}
