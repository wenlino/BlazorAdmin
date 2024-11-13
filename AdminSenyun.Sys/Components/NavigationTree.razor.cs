using AdminSenyun.Sys.Extensions;

namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class NavigationTree
{
    [NotNull]
    private List<TreeViewItem<SysNavigation>>? Items { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SysNavigation>? AllMenus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<long>? SelectedMenus { get; set; }

    /// <summary>
    /// 保存按钮回调委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Func<List<long>, Task<bool>>? OnSave { get; set; }

    [CascadingParameter]
    private Func<Task>? CloseDialogAsync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Items = AllMenus.ToTreeItemList(SelectedMenus, RenderTreeItem);
    }

    private async Task OnClickClose()
    {
        if (CloseDialogAsync != null)
        {
            await CloseDialogAsync();
        }
    }

    private Task OnTreeItemChecked(List<TreeViewItem<SysNavigation>> items)
    {
        List<long> addPid(TreeViewItem<SysNavigation> item)
        {
            var navs = new List<long>();
            if (item.Parent != null)
            {
                navs.AddRange(addPid(item.Parent));
            }
            navs.Add(item.Value.Id);
            return navs;
        };

        SelectedMenus = items.SelectMany(addPid).Distinct().ToList();

        return Task.CompletedTask;
    }

    private async Task OnClickSave()
    {
        var ret = await OnSave(SelectedMenus);
        if (ret)
        {
            await OnClickClose();
            await ToastService.Success("分配菜单操作", "操作成功！");

        }
        else
        {
            await ToastService.Error("分配菜单操作", "操作失败，请联系相关管理员！");
        }
    }
}
