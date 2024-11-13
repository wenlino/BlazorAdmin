using AdminSenyun.Sys.Components;
using AdminSenyun.Sys.Extensions;

namespace AdminSenyun.Sys.Extensions;

/// <summary>
/// 
/// </summary>
public static class DialogExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static Task ShowAssignmentDialog(this DialogService dialogService, string title, List<SelectedItem> items, List<long> value, Func<Task<bool>> saveCallback, ToastService? toast) => dialogService.ShowDialog<Assignment, SelectedItem>(title, items, value, saveCallback, toast);

    /// <summary>
    /// 弹出菜单分配弹窗
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="title">弹窗标题</param>
    /// <param name="menus">当前用户可用所有菜单集合</param>
    /// <param name="value">已分配菜单集合</param>
    /// <param name="saveCallback"></param>
    /// <returns></returns>
    public static async Task ShowNavigationDialog(this DialogService dialogService, string title, List<SysNavigation> menus, List<long> value, Func<List<long>, Task<bool>> saveCallback)
    {
        var option = new DialogOption
        {
            Title = title,
            ShowFooter = false,
            Class = "modal-dialog-menu"
        };
        var parameters = new Dictionary<string, object?>()
        {
            [nameof(NavigationTree.AllMenus)] = menus,
            [nameof(NavigationTree.SelectedMenus)] = value,
            [nameof(NavigationTree.OnSave)] = new Func<List<long>, Task<bool>>(items => saveCallback(items))
        };
        option.Component = BootstrapDynamicComponent.CreateComponent<NavigationTree>(parameters);
        await dialogService.Show(option);
    }

    private static Task ShowDialog<TBody, TItem>(this DialogService dialogService,
        string title, List<TItem> items,
        List<long> value, Func<Task<bool>> saveCallback,
        ToastService? toast) where TBody : AssignmentBase<TItem>
        => dialogService.ShowSaveDialog<TBody>(title, async () =>
    {
        var ret = await saveCallback();
        if (toast != null)
        {
            if (ret)
            {
                await toast.Success("分配操作", "操作成功！");
            }
            else
            {
                await toast.Error("分配操作", "操作失败，请联系相关管理员！");
            }
        }
        return ret;
    },
    parameters =>
    {
        parameters.Add(nameof(AssignmentBase<TItem>.Items), items);
        parameters.Add(nameof(AssignmentBase<TItem>.Value), value);
        parameters.Add(nameof(AssignmentBase<TItem>.OnValueChanged), new Action<List<long>>(v =>
        {
            value.Clear();
            value.AddRange(v);
        }));
    },
    op =>
    {
        op.IsScrolling = true;
    });
}
