namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
[CascadingTypeParameter(nameof(TItem))]
public partial class SysAdminTable<TItem> where TItem : class, new()
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public IEnumerable<int>? PageItemsSource { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public int ExtendButtonColumnWidth { get; set; } = 130;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? SortString { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    [Parameter]
    public RenderFragment<TItem>? TableColumns { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? RowButtonTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment<ITableSearchModel>? CustomerSearchTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? EditTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    [Parameter]
    public RenderFragment? TableToolbarTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool IsPagination { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool IsMultipleSelect { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool IsFixedHeader { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool IsTree { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool ShowToolbar { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool ShowEmpty { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool ShowLoading { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool ShowSearch { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool ShowAdvancedSearch { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool ShowDefaultButtons { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool ShowExtendButtons { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool AutoGenerateColumns { get; set; }
    /// <summary>
    /// 获得/设置 默认每页数据数量 默认 0 使用 BootstrapBlazor.Components.Table`1.PageItemsSource 第一个值
    /// </summary>
    [Parameter]
    public int PageItems { get; set; } = 20;
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ITableSearchModel? CustomerSearchModel { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<QueryPageOptions, Task<QueryData<TItem>>>? OnQueryAsync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<TItem, Task<IEnumerable<TableTreeNode<TItem>>>>? OnTreeExpand { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<IEnumerable<TItem>, Task<IEnumerable<TableTreeNode<TItem>>>>? TreeNodeConverter { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<TItem, ItemChangedType, Task<bool>>? OnSaveAsync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<IEnumerable<TItem>, Task<bool>>? OnDeleteAsync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public List<TItem>? SelectedRows { get; set; } = new List<TItem>();

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<TItem, bool>? ShowEditButtonCallback { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<TItem, bool>? ShowDeleteButtonCallback { get; set; }

    /// <summary>
    /// 获得/设置 保存删除后回调委托方法
    /// </summary>
    [Parameter]
    public Func<Task>? OnAfterModifyAsync { get; set; }

    /// <summary>
    /// 获得/设置 新建模型回调方法 默认 null 未设置时使用默认无参构造函数创建
    /// </summary>
    [Parameter]
    public Func<TItem>? CreateItemCallback { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<TItem, TItem, bool>? ModelEqualityComparer { get; set; }

    [NotNull]
    private Table<TItem>? Instance { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ValueTask ToggleLoading(bool v) => Instance.ToggleLoading(v);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task QueryAsync() => Instance.QueryAsync();



    [Inject]
    [NotNull]
    private IAdmin? AdminService { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    private bool AuthorizeButton(string operate)
    {
        var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        return AdminService.AuthorizingBlock(AdminService.UserName, url, operate);
    }
}
