namespace AdminSenyun.Data.Core;

/// <summary>
/// 
/// </summary>
public interface IGroup
{
    /// <summary>
    /// 获得所有用户
    /// </summary>
    /// <returns></returns>
    List<SysGroup> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    List<long> GetGroupsByUserId(long userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    bool SaveGroupsByUserId(long userId, IEnumerable<long> groupIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<long> GetGroupsByRoleId(long roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    bool SaveGroupsByRoleId(long roleId, IEnumerable<long> groupIds);
}
