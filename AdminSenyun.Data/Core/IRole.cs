namespace AdminSenyun.Data.Core;

/// <summary>
/// 
/// </summary>
public interface IRole
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<SysRole> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    List<long> GetRolesByGroupId(long groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByGroupId(long groupId, IEnumerable<long> roleIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    List<long> GetRolesByUserId(long userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByUserId(long userId, IEnumerable<long> roleIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    List<long> GetRolesByMenuId(long menuId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByMenuId(long menuId, IEnumerable<long> roleIds);

    /// <summary>
    /// 清理缓存
    /// </summary>
    void ClearCache();
}
