using AdminSenyun.Data.Core;
using AdminSenyun.Data.Models;

namespace AdminSenyun.Data.Service;

class GroupService(ISqlSugarClient db) : IGroup
{
    public List<SysGroup> GetAll() => db.Queryable<SysGroup>().ToList();

    public List<long> GetGroupsByRoleId(long roleId) => db.Queryable<SysRoleGroup>().Where(t => t.RoleID == roleId).Select(t => t.GroupID!).ToList();

    public List<long> GetGroupsByUserId(long userId) => db.Queryable<SysUserGroup>().Where(t => t.UserID == userId).Select(t => t.GroupID!).ToList();

    public bool SaveGroupsByRoleId(long roleId, IEnumerable<long> groupIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysRoleGroup>().Where(t => t.RoleID == roleId).ExecuteCommand();
            db.Insertable(groupIds.Select(g => new SysRoleGroup { GroupID = g, RoleID = roleId }).ToList()).ExecuteCommand();
            ret = true;
            db.Ado.CommitTran();
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveGroupsByUserId(long userId, IEnumerable<long> groupIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysUserGroup>().Where(t => t.UserID == userId).ExecuteCommand();
            db.Insertable(groupIds.Select(g => new SysUserGroup { GroupID = g, UserID = userId }).ToList()).ExecuteCommand();
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
}
