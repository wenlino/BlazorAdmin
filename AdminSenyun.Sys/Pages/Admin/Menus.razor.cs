using AdminSenyun.Data.Service;
using AdminSenyun.Sys.Components;
using AdminSenyun.Sys.Extensions;


namespace AdminSenyun.Sys.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Menus
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IRole? RoleService { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private IAdmin? AdminService { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    [Inject, NotNull]
    private ICache? cache { get; set; }

    [NotNull]
    private List<SelectedItem>? Targets { get; set; }


    [NotNull]
    private List<SelectedItem>? ParamentMenus { get; set; }

    [CascadingParameter]
    private Func<Task>? ReloadMenu { get; set; }

    private ITableSearchModel? SearchModel { get; set; } = new MenusSearchModel();

    private SysAdminTable<SysNavigation> tableRef { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Initcom();
    }


    private void Initcom()
    {
        Targets = [
            new SelectedItem("_self", "本窗口"),
            new SelectedItem("_blank", "新窗口"),
            new SelectedItem("_parent", "父级窗口"),
            new SelectedItem("_top", "顶级窗口"),
        ];

        //ParamentMenus = NavigationService.GetAllMenus(AdminService.UserName).Where(s => s.ParentId == 0).Select(s => new SelectedItem(s.Id.ToString(), s.Name)).ToList();
        ParamentMenus = NavigationService.GetAllMenus(AdminService.UserName).Select(s => new SelectedItem(s.Id.ToString(), s.Name)).ToList();
        ParamentMenus.Insert(0, new SelectedItem("0", "请选择"));
    }

    private bool AuthorizeButton(string operate)
    {
        return AdminService.IsBlock(operate);
        //var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        //return AdminService.AuthorizingBlock(AppContext.UserName, url, operate);
    }

    private async Task OnAssignmentRoles(SysNavigation menu)
    {
        var roles = RoleService.GetAll().ToSelectedItemList();
        var values = RoleService.GetRolesByMenuId(menu.Id);

        await DialogService.ShowAssignmentDialog($"分配角色 - 当前菜单: {menu.Name}", roles, values, () =>
        {
            var ret = RoleService.SaveRolesByMenuId(menu.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task<bool> OnDeleteAsync(IEnumerable<SysNavigation> sysNavigations)
    {
        var ret = await NavigationService.DelteMenusAsync(sysNavigations);
        if (!ret)
        {
            await ToastService.Error("出错", "检查菜单是否存在子项，请全部删除后再删除");
        }
        return ret;
    }

    private async Task OnAddMenusSubRole(SysNavigation menu)
    {
        var add = new SysNavigation()
        {
            ParentId = menu.Id,
            Name = "新增",
            Order = 100,
            Icon = "fa-solid fa-flag",
            Url = "add",
            Target = "_self",
            IsResource = EnumResource.Block,
        };
        var edit = new SysNavigation()
        {
            ParentId = menu.Id,
            Name = "编辑",
            Order = 100,
            Icon = "fa-solid fa-flag",
            Url = "edit",
            Target = "_self",
            IsResource = EnumResource.Block,
        };
        var delete = new SysNavigation()
        {
            ParentId = menu.Id,
            Name = "删除",
            Order = 100,
            Icon = "fa-solid fa-flag",
            Url = "del",
            Target = "_self",
            IsResource = EnumResource.Block,
        };

        List<SysNavigation> menus = [add, edit, delete];
        NavigationService.SaveMenus(menus);
        await tableRef.QueryAsync();
    }

    private Task<QueryData<SysNavigation>> OnQueryAsync(QueryPageOptions options)
    {
        var navigations = NavigationService.GetAllMenus(AdminService.UserName);
        var menus = navigations.Where(m => m.ParentId == 0);

        // 处理模糊查询
        if (options.Searches.Any())
        {
            menus = menus.Where(options.Searches.GetFilterFunc<SysNavigation>(FilterLogic.Or));
        }

        //  处理 Filter 高级搜索
        if (options.CustomerSearches.Any() || options.Filters.Any())
        {
            menus = menus.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<SysNavigation>());
        }

        foreach (var item in menus)
        {
            item.HasChildren = navigations.Any(i => i.ParentId == item.Id);
        }

        return Task.FromResult(new QueryData<SysNavigation>()
        {
            IsFiltered = true,
            IsSearch = true,
            IsSorted = true,
            Items = menus
        });
    }

    private async Task OnAfterModifyAsync()
    {
        if (ReloadMenu != null)
        {
            await ReloadMenu();
        }
        cache.ClearAll();
        Initcom();
    }

    private Task<IEnumerable<TableTreeNode<SysNavigation>>> OnTreeExpand(SysNavigation menu)
    {
        var navigations = NavigationService.GetAllMenus(AdminService.UserName);
        return Task.FromResult(navigations.Where(m => m.ParentId == menu.Id).OrderBy(m => m.Order).AsEnumerable().Select(i => new TableTreeNode<SysNavigation>(i)));
    }

    private Task<IEnumerable<TableTreeNode<SysNavigation>>> TreeNodeConverter(IEnumerable<SysNavigation> items)
    {
        var ret = BuildTreeNodes(items, "0");
        return Task.FromResult(ret);

        IEnumerable<TableTreeNode<SysNavigation>> BuildTreeNodes(IEnumerable<SysNavigation> items, string parentId)
        {
            var navigations = NavigationService.GetAllMenus(AdminService.UserName);
            var ret = new List<TableTreeNode<SysNavigation>>();
            ret.AddRange(items.Where(i => i.ParentId.ToString() == parentId).Select((nav, index) => new TableTreeNode<SysNavigation>(nav)
            {
                HasChildren = navigations.Any(i => i.ParentId == nav.Id),
                //IsExpand = navigations.Any(i => i.ParentId == nav.Id),
                Items = BuildTreeNodes(navigations.Where(i => i.ParentId == nav.Id), nav.Id.ToString())
            }));
            return ret;
        }
    }

    private bool ModelEqualityComparer(SysNavigation x, SysNavigation y) => x.Id == y.Id;
}
