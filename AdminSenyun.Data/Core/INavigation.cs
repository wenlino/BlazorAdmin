namespace AdminSenyun.Data.Core;

/// <summary>
/// 
/// </summary>
public interface INavigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<SysNavigation> GetAllMenus(string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<long> GetMenusByRoleId(long roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="menuIds"></param>
    /// <returns></returns>
    bool SaveMenusByRoleId(long roleId, List<long> menuIds);

    /// <summary>
    /// 保存菜单
    /// </summary>
    /// <param name="navigations"></param>
    /// <returns></returns>
    bool SaveMenus(List<SysNavigation> navigations);

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="navigations"></param>
    /// <returns></returns>
    Task<bool> DelteMenusAsync(IEnumerable<SysNavigation> navigations);
}
