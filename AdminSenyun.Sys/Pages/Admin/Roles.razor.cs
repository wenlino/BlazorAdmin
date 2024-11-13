using AdminSenyun.Data.Service;
using AdminSenyun.Sys.Extensions;


namespace AdminSenyun.Sys.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Roles
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IGroup? GroupService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }
    
    [Inject]
    [NotNull]
    private IAdmin? AdminService { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }


    private async Task OnAssignmentUsers(SysRole role)
    {
        var users = UserService.GetAll().ToSelectedItemList();
        var values = UserService.GetUsersByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"分配用户 - 当前角色: {role.RoleName}", users, values, () =>
        {
            var ret = UserService.SaveUsersByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentGroups(SysRole role)
    {
        var groups = GroupService.GetAll().ToSelectedItemList();
        var values = GroupService.GetGroupsByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"分配部门 - 当前角色: {role.RoleName}", groups, values, () =>
        {
            var ret = GroupService.SaveGroupsByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentMenus(SysRole role)
    {
        var menus = NavigationService.GetAllMenus(AdminService.UserName);
        var values = NavigationService.GetMenusByRoleId(role.Id);

        await DialogService.ShowNavigationDialog($"分配菜单 - 当前角色: {role.RoleName}", menus, values, items =>
        {
            var ret = NavigationService.SaveMenusByRoleId(role.Id, items);
            return Task.FromResult(ret);
        });
    }
}
