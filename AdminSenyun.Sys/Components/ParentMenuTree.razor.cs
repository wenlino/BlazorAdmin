using AdminSenyun.Sys.Extensions;

namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class ParentMenuTree
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public long Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<long> ValueChanged { get; set; }

    [NotNull]
    private List<TreeViewItem<SysNavigation>>? Items { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject, NotNull]
    private IAdmin admin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var items = NavigationService.GetAllMenus(admin.UserName);
        Items = items.ToTreeItemList([Value], RenderTreeItem);
    }

    private async Task OnTreeItemClick(TreeViewItem<SysNavigation> item)
    {
        Value = item.Value.Id;
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
