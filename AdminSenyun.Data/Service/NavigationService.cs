using AdminSenyun.Data.Core;
using AdminSenyun.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminSenyun.Data.Service;

class NavigationService(ISqlSugarClient db, ICache cache) : INavigation
{
    public List<SysNavigation> GetAllMenus(string userName)
    {
        var getitems = () =>
        {
            var items = db.Queryable<SysNavigation>().Where(n =>
                //获取用户权限 菜单列表
                SqlFunc.Subqueryable<SysUser>()
                .InnerJoin<SysUserRole>((u, ur) => u.Id == ur.UserID)
                .InnerJoin<SysNavigationRole>((u, ur, nr) => nr.RoleID == ur.RoleID)
                .Where(u => u.UserName == userName)
                .Where((u, ur, nr) => nr.NavigationID == n.Id).Any() ||
                //获取用户组角色组权限 菜单列表
                SqlFunc.Subqueryable<SysUser>()
                .InnerJoin<SysUserGroup>((u, ug) => u.Id == ug.GroupID)
                .InnerJoin<SysRoleGroup>((u, ug, rg) => rg.GroupID == ug.GroupID)
                .InnerJoin<SysNavigationRole>((u, ug, rg, nr) => nr.RoleID == rg.RoleID)
                .Where(u => u.UserName == userName)
                .Where((u, ug, rg, nr) => nr.NavigationID == n.Id).Any() ||
                //是否是超级管理员身份 超级管理获取全部菜单
                SqlFunc.Subqueryable<SysNavigation>().Where(t =>
                    SqlFunc.Subqueryable<SysUser>()
                    .InnerJoin<SysUserRole>((u, ur) => u.Id == ur.UserID)
                    .InnerJoin<SysRole>((u, ur, r) => ur.RoleID == r.Id)
                    .Where((u, ur, r) => u.UserName == userName && r.RoleName == "Administrators").Any()
                ).Where(t => t.Id == n.Id).Any()
            ).OrderBy(n => n.Order)
            .OrderBy(n => n.Id)
            .ToList();
            return items;
        };

        //使用数据缓存服务
        var ns = cache.GetOrCreate($"{nameof(SysNavigation)}_{userName}", factory => getitems());

        return ns;
    }

    public List<long> GetMenusByRoleId(long roleId) => db.Queryable<SysNavigationRole>().Where(t => t.RoleID == roleId).Select(t => t.NavigationID!).ToList();

    public bool SaveMenus(List<SysNavigation> navigations)
    {
        cache.ClearAll();
        return db.Storageable(navigations).ExecuteCommand() > 0;
    }

    public bool SaveMenusByRoleId(long roleId, List<long> menuIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<SysNavigationRole>().Where(t => t.RoleID == roleId).ExecuteCommand();
            db.Insertable(menuIds.Select(g => new SysNavigationRole { NavigationID = g, RoleID = roleId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
            cache.ClearAll();
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public async Task<bool> DelteMenusAsync(IEnumerable<SysNavigation> navigations)
    {
        var ids = navigations.Select(t => t.Id);

        var result = db.Queryable<SysNavigation>().Where(t => ids.Contains(t.ParentId)).Any();

        if (result)
        {
            return false;
        }
        else
        {
            return db.Deleteable<SysNavigation>(navigations).ExecuteCommand() > 0;
        }
    }
}
