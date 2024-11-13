using AdminSenyun.Data.Core;
using AdminSenyun.Data.Models;
using AdminSenyun.Models;


namespace AdminSenyun.Data.Service;

class UserService(ISqlSugarClient db, ICache cache) : IUser
{
    public bool Authenticate(string userName, string password)
    {
        var user = db.Queryable<SysUser>()
            .Where(s => s.ApprovedTime != null && s.UserName == userName)
            .Select(s => new SysUser
            {
                DisplayName = s.DisplayName,
                PassSalt = s.PassSalt,
                Password = s.Password
            }).Single();

        var isAuth = false;
        if (user != null && !string.IsNullOrEmpty(user.PassSalt))
        {
            isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }
        return isAuth;
    }

    public List<SysUser> GetAll()
    {
        return db.Queryable<SysUser>().ToList();
    }


    public string? GetDisplayName(string? userName) => db.Queryable<SysUser>().Where(s => s.UserName == userName).Select(s => s.DisplayName).Single();

    public List<string> GetRoles(string userName)
    {
        var getroles = () => db.Union(
            db.Queryable<SysRole>()
            .InnerJoin<SysUserRole>((r, ur) => r.Id == ur.RoleID)
            .InnerJoin<SysUser>((r, ur, u) => ur.UserID == u.Id && u.UserName == userName)
            .Select(r => r.RoleName),
            db.Queryable<SysRole>()
            .InnerJoin<SysRoleGroup>((r, rg) => r.Id == rg.RoleID)
            .InnerJoin<SysGroup>((r, rg, g) => rg.GroupID == g.Id)
            .InnerJoin<SysUserGroup>((r, rg, g, ug) => ug.GroupID == g.Id)
            .InnerJoin<SysUser>((r, rg, g, ug, u) => ug.UserID == u.Id && u.UserName == userName)
            .Select(r => r.RoleName)
        ).ToList();

        var roles = cache.GetOrCreate($"Role_{userName}", factory => getroles());

        return roles;
    }

    public SysUser? GetUserByUserName(string? userName)
    {
        var getuser = () => string.IsNullOrEmpty(userName)
        ? null : db.Queryable<SysUser>().Where(t => t.UserName == userName).Single();
        return cache.GetOrCreate($"User_{userName}", factory => getuser());
    }

    public List<long> GetUsersByGroupId(long groupId) => db.Queryable<SysUserGroup>()
        .Where(t => t.GroupID == groupId)
        .Select(t => t.UserID!)
        .ToList();

    public List<long> GetUsersByRoleId(long roleId) => db.Queryable<SysUserRole>()
        .Where(t => t.RoleID == roleId)
        .Select(t => t.UserID!)
        .ToList();

    public bool SaveUser(string userName, string displayName, string password)
    {
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        var user = db.Queryable<SysUser>().Where(s => s.UserName == userName).Single();
        bool ret = default;
        if (user == null)
        {
            try
            {
                // 开始事务
                db.Ado.BeginTran();
                user = new SysUser()
                {
                    ApprovedBy = "System",
                    ApprovedTime = DateTime.Now,
                    DisplayName = displayName,
                    UserName = userName,
                    Icon = "default.png",
                    Description = "系统默认创建",
                    PassSalt = salt,
                    Password = pwd
                };
                db.Insertable(user).ExecuteCommand();

                // 授权 Default 角色
                var urs = db.Queryable<SysUser>()
                    .Where(t => t.UserName == userName)
                    .Select(t => new SysUserRole()
                    {
                        UserID = t.Id,
                        RoleID = SqlFunc.Subqueryable<SysRole>().Where(x => x.RoleName == "Default").Select(x => x.Id)
                    }).ToList();

                db.Insertable(urs).ExecuteCommand();

                ret = true;
                db.Ado.CommitTran();
            }
            finally
            {
                db.Ado.RollbackTran();
            }
        }
        else
        {

            user.DisplayName = displayName;
            user.PassSalt = salt;
            user.Password = pwd;
            db.Updateable(user).ExecuteCommand();
            ret = true;
        }
        return ret;
    }

    public bool SaveUsersByGroupId(long groupId, IEnumerable<long> userIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysUserGroup>().Where(x => x.GroupID == groupId).ExecuteCommand();
            db.Insertable(userIds.Select(g => new SysUserGroup { UserID = g, GroupID = groupId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveUsersByRoleId(long roleId, IEnumerable<long> userIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysUserRole>().Where(x => x.RoleID == roleId).ExecuteCommand();
            db.Insertable(userIds.Select(g => new SysUserRole { UserID = g, RoleID = roleId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public bool TryCreateUserByPhone(string phone, string code, ICollection<string> roles)
    {
        var ret = false;
        try
        {
            var salt = LgbCryptography.GenerateSalt();
            var pwd = LgbCryptography.ComputeHash(code, salt);
            var user = db.Queryable<SysUser>().Where(s => s.UserName == phone).Single();
            if (user == null)
            {
                try
                {
                    db.Ado.BeginTran();
                    user = new SysUser()
                    {
                        ApprovedBy = "Mobile",
                        ApprovedTime = DateTime.Now,
                        DisplayName = "手机用户",
                        UserName = phone,
                        Icon = "default.png",
                        Description = "手机用户",
                        PassSalt = salt,
                        Password = LgbCryptography.ComputeHash(code, salt),
                    };
                    db.Insertable(user).ExecuteCommand();
                    // Authorization
                    var roleIds = db.Queryable<SysRole>().Where(t => roles.Contains(t.RoleName)).Select(t => t.Id).ToList();
                    db.Insertable(roleIds.Select(g => new SysUserRole { RoleID = g, UserID = user.Id }).ToList()).ExecuteCommand();
                    db.Ado.CommitTran();
                }
                finally
                {
                    db.Ado.RollbackTran();
                }
            }
            else
            {
                user.PassSalt = salt;
                user.Password = pwd;
                db.Updateable(user).ExecuteCommand();
            }
            ret = true;
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveApp(string userName, string app)
    {
        var ret = db.Updateable<SysUser>().SetColumns(t => t.App == app).Where(t => t.UserName == userName).ExecuteCommand() > 0;
        if (ret)
        {
            cache.Remove<SysDict>();
        }
        return ret;
    }

    public bool ChangePassword(string userName, string password, string newPassword)
    {
        var ret = false;
        if (Authenticate(userName, password))
        {
            var passSalt = LgbCryptography.GenerateSalt();
            password = LgbCryptography.ComputeHash(newPassword, passSalt);
            ret = db.Updateable<SysUser>()
                .SetColumns(t => t.Password == password)
                .SetColumns(t => t.PassSalt == passSalt)
                .Where(t => t.UserName == userName)
                .ExecuteCommand() > 0;
        }
        return ret;
    }

    public bool SaveDisplayName(string userName, string displayName)
    {
        var ret = db.Updateable<SysUser>()
            .SetColumns(t => t.DisplayName == displayName)
            .Where(t => t.UserName == userName)
            .ExecuteCommand() > 0;
        if (ret)
        {
            cache.Remove<SysDict>();
        }
        return ret;
    }

    public bool SaveTheme(string userName, string theme)
    {
        var ret = db.Updateable<SysUser>()
            .SetColumns(t => t.Css == theme)
            .Where(t => t.UserName == userName)
            .ExecuteCommand() > 0;
        if (ret)
        {
            cache.Remove<SysDict>();
        }
        return ret;
    }

    public bool SaveLogo(string userName, string? logo)
    {
        var ret = db.Updateable<SysUser>()
            .SetColumns(t => t.Icon == logo)
            .Where(t => t.UserName == userName)
            .ExecuteCommand() > 0;
        if (ret)
        {
            cache.Remove<SysDict>();
        }
        return ret;
    }

    public bool SaveLoginLog(SysLoginLog sysLoginLog)
    {
        return db.Insertable(sysLoginLog).ExecuteCommand() > 0;
    }
}
