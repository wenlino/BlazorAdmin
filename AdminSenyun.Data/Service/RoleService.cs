using AdminSenyun.Data.Core;
using AdminSenyun.Data.Models;
using Stimulsoft.System.Web.Caching;

namespace AdminSenyun.Data.Service;

class RoleService(ISqlSugarClient db, ICache cache) : IRole
{
    public List<SysRole> GetAll() => cache.GetAll<SysRole>();
    //public List<Role> GetAll() => db.Queryable<Role>().ToList();
    public List<SysRole> Roles => GetAll();

    public void ClearCache()
    {
        cache.Remove<SysRole>();
        cache.Remove<SysRoleGroup>();
        cache.Remove<SysNavigationRole>();
        cache.Remove<SysUserRole>();
    }


    public List<long> GetRolesByGroupId(long groupId) =>
        cache.GetAll<SysRoleGroup>().Where(t => t.GroupID == groupId).Select(t => t.RoleID!).ToList();
    //db.Queryable<RoleGroup>().Where(t => t.GroupID == groupId).Select(t => t.RoleID!).ToList();

    public List<long> GetRolesByMenuId(long menuId) =>
        cache.GetAll<SysNavigationRole>().Where(t => t.NavigationID == menuId).Select(t => t.RoleID!).ToList();
    //db.Queryable<NavigationRole>().Where(t => t.NavigationID == menuId).Select(t => t.RoleID!).ToList();

    public List<long> GetRolesByUserId(long userId) =>
        cache.GetAll<SysUserRole>().Where(t => t.UserID == userId).Select(t => t.RoleID!).ToList();
    //db.Queryable<UserRole>().Where(t => t.UserID == userId).Select(t => t.RoleID!).ToList();

    public bool SaveRolesByGroupId(long groupId, IEnumerable<long> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysRoleGroup>().Where(t => t.GroupID == groupId).ExecuteCommand();
            db.Insertable(roleIds.Select(g => new SysRoleGroup { RoleID = g, GroupID = groupId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
            ClearCache();
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveRolesByMenuId(long menuId, IEnumerable<long> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysNavigationRole>().Where(t => t.NavigationID == menuId).ExecuteCommand();
            db.Insertable(roleIds.Select(g => new SysNavigationRole { RoleID = g, NavigationID = menuId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
            ClearCache();
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveRolesByUserId(long userId, IEnumerable<long> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysUserRole>().Where(t => t.UserID == userId).ExecuteCommand();
            db.Insertable(roleIds.Select(g => new SysUserRole { RoleID = g, UserID = userId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
            ClearCache();
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }
}
