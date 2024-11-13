namespace AdminSenyun.Data.Core;

/// <summary>
/// 
/// </summary>
public interface IUser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    SysUser? GetUserByUserName(string? userName);

    /// <summary>
    /// 通过用户名获取角色列表
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    List<string> GetRoles(string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    List<long> GetUsersByGroupId(long groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    bool SaveUsersByGroupId(long groupId, IEnumerable<long> userIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<long> GetUsersByRoleId(long roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    bool SaveUsersByRoleId(long roleId, IEnumerable<long> userIds);

    /// <summary>
    /// 更新密码方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    bool ChangePassword(string userName, string password, string newPassword);

    /// <summary>
    /// 保存显示名称方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    bool SaveDisplayName(string userName, string displayName);

    /// <summary>
    /// 保存用户主题方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="theme"></param>
    /// <returns></returns>
    bool SaveTheme(string userName, string theme);

    /// <summary>
    /// 保存用户头像方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="logo"></param>
    /// <returns></returns>
    bool SaveLogo(string userName, string? logo);

    /// <summary>
    /// 获得所有用户
    /// </summary>
    /// <returns></returns>
    List<SysUser> GetAll();


    /// <summary>
    /// 认证方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    bool Authenticate(string userName, string password);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    bool TryCreateUserByPhone(string phone, string code, ICollection<string> roles);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    bool SaveUser(string userName, string displayName, string password);

    /// <summary>
    /// 保存登录日志
    /// </summary>
    /// <param name="sysLoginLog"></param>
    /// <returns></returns>
    bool SaveLoginLog(SysLoginLog sysLoginLog);
}
