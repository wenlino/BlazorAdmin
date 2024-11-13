using SqlSugar;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using BootstrapBlazor.Components;
using AdminSenyun.Models;
using AdminSenyun.Data.Core;


namespace AdminSenyun.Core.Components;

/// <summary>
/// 
/// </summary>
[CascadingTypeParameter(nameof(TItem))]
[JSModuleAutoLoader("./_content/AdminSenyun.Core/Components/AdminTable.razor.js", JSObjectReference = true)]
public partial class AdminTable<TItem> where TItem : class, new()
{
    /// <summary>
    /// 获得/设置 每页显示数据数量的外部数据源
    /// </summary>
    [Parameter]
    //public IEnumerable<int>? PageItemsSource { get; set; } = new List<int>() { 10, 20, 50, 100, 1000 };
    public IEnumerable<int>? PageItemsSource { get; set; } = [10, 20, 50, 100, 1000];

    /// <summary>
    /// 获得/设置 默认每页数据数量 默认 0 使用 BootstrapBlazor.Components.Table`1.PageItemsSource 第一个值
    /// </summary>
    [Parameter]
    public int PageItems { get; set; } = 10;

    /// <summary>
    /// 获得/设置 行内操作列宽度 默认为 130
    /// </summary>
    [Parameter]
    public int ExtendButtonColumnWidth { get; set; } = 130;

    /// <summary>
    /// 获得/设置 多列排序顺序 默认为空 多列时使用逗号分割 如："Name, Age desc"
    /// </summary>
    [Parameter]
    public string? SortString { get; set; }

    /// <summary>
    /// 获得/设置 TableHeader 实例
    /// </summary>
    [NotNull]
    [Parameter]
    public RenderFragment<TItem>? TableColumns { get; set; }

    /// <summary>
    /// 获得/设置 RowButtonTemplate 实例 此模板生成的按钮默认放置到按钮后面如需放置前面 请查看 BootstrapBlazor.Components.Table`1.BeforeRowButtonTemplate
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? RowButtonTemplate { get; set; }

    /// <summary>
    /// 获得/设置 自定义搜索模型模板 BootstrapBlazor.Components.Table`1.CustomerSearchModel
    /// </summary>
    [Parameter]
    public RenderFragment<ITableSearchModel>? CustomerSearchTemplate { get; set; }

    /// <summary>
    /// 获得/设置 EditTemplate 实例
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? EditTemplate { get; set; }

    /// <summary>
    /// 获得/设置 表格 Toolbar 按钮模板
    /// 表格工具栏左侧按钮模板，模板中内容出现在默认按钮后面
    /// </summary>
    [NotNull]
    [Parameter]
    public RenderFragment? TableToolbarTemplate { get; set; }

    /// <summary>
    /// 获得/设置 是否分页 默认为 false
    /// </summary>
    [Parameter]
    public bool IsPagination { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否为多选模式 默认为 false  
    /// 此参数在 BootstrapBlazor.Components.Table`1.IsExcel 模式下为 true
    /// </summary>
    [Parameter]
    public bool IsMultipleSelect { get; set; } = true;

    /// <summary>
    /// 获得/设置 固定表头 默认 true
    /// </summary>
    [Parameter]
    public bool IsFixedHeader { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否为树形数据 默认为 false
    /// </summary>
    [Parameter]
    public bool IsTree { get; set; }

    /// <summary>
    ///   获得/设置 是否显示工具栏 默认 true 显示
    /// </summary>
    [Parameter]
    public bool ShowToolbar { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否显示无数据空记录 默认 false 不显示
    /// </summary>
    [Parameter]
    public bool ShowEmpty { get; set; } = true;

    /// <summary>
    /// 获得/设置 查询时是否显示正在加载中动画 默认为 false
    /// </summary>
    [Parameter]
    public bool ShowLoading { get; set; } = false;

    /// <summary>
    /// 获得/设置 是否显示搜索框 默认为 false 不显示搜索框
    /// </summary>
    [Parameter]
    public bool ShowSearch { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否显示高级搜索按钮 默认 true 显示 BootstrapBlazor.Components.Table`1.ShowSearch
    /// </summary>
    [Parameter]
    public bool ShowAdvancedSearch { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否显示按钮列 默认为 true
    /// 本属性设置为 true 新建编辑删除按钮设置为 false 可单独控制每个按钮是否显示
    /// </summary>
    [Parameter]
    public bool ShowDefaultButtons { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否显示扩展按钮 默认为 false
    /// </summary>
    [Parameter]
    public bool ShowExtendButtons { get; set; } = true;

    /// <summary>
    ///  获得/设置 是否显示导出按钮
    /// </summary>
    [Parameter]
    public bool ShowExportButton { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否显示 Csv 导出按钮
    /// </summary>
    [Parameter]
    public bool ShowExportCsvButton { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否显示 Pdf 导出按钮
    /// </summary>
    [Parameter]
    public bool ShowExportPdfButton { get; set; }

    /// <summary>
    /// 获得/设置 是否自动生成列信息
    /// </summary>
    [Parameter]
    public bool AutoGenerateColumns { get; set; }

    /// <summary>
    /// 获得/设置 是否固定扩展按钮列 默认为 false 不固定
    /// </summary>
    [Parameter]
    public bool FixedExtendButtonsColumn { get; set; } = false;

    /// <summary>
    /// 获得/设置 点击行即选中本行 默认为 true
    /// </summary>
    [Parameter]
    public bool ClickToSelect { get; set; } = true;

    /// <summary>
    /// 获得/设置 客户端表格名称 默认 null 用于客户端列宽与列顺序持久化功能
    /// </summary>
    [Parameter]
    public string? ClientTableName { get; set; }

    /// <summary>
    /// 获得/设置 编辑按钮文本
    /// </summary>
    [Parameter]
    public string? EditButtonText { get; set; } = "编辑";

    /// <summary>
    /// 获得/设置 新建按钮文本
    /// </summary>
    [Parameter]
    public string? AddButtonText { get; set; } = "新建";

    /// <summary>
    /// 获得/设置 删除按钮文本
    /// </summary>
    [Parameter]
    public string? DeleteButtonText { get; set; } = "删除";

    /// <summary>
    /// 获得/设置 Table 高度 默认为 null
    /// </summary>
    [Parameter]
    public int? Height { get; set; }



    /// <summary>
    /// 获得/设置 是否显示新建按钮 默认为 true 显示
    /// </summary>
    [Parameter]
    public bool? ShowAddButton { get; set; }


    /// <summary>
    /// 获得/设置 是否显示删除按钮 默认为 true 行内是否显示请使用 BootstrapBlazor.Components.Table`1.ShowExtendDeleteButton与 BootstrapBlazor.Components.Table`1.ShowExtendDeleteButtonCallback
    /// </summary>
    [Parameter]
    public bool? ShowDeleteButton { get; set; }

    /// <summary>
    /// 获得/设置 是否显示编辑按钮 默认为 true 行内是否显示请使用 ootstrapBlazor.Components.Table`1.ShowExtendEditButton与 BootstrapBlazor.Components.Table`1.ShowExtendEditButtonCallback
    /// </summary>
    [Parameter]
    public bool? ShowEditButton { get; set; }


    /// <summary>
    /// 获得/设置 数据集合，适用于无功能仅做数据展示使用，高级功能时请使用 BootstrapBlazor.Components.Table`1.OnQueryAsync
    /// </summary>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }
    /// <summary>
    /// 获得/设置 数据集合回调方法
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TItem>> ItemsChanged { get; set; }

    /// <summary>
    /// 获得/设置 自定义搜索模型 BootstrapBlazor.Components.Table`1.CustomerSearchTemplate
    /// </summary>
    [Parameter]
    public ITableSearchModel? CustomerSearchModel { get; set; }

    /// <summary>
    /// 异步查询回调方法，设置 BootstrapBlazor.Components.Table`1.Items 后无法触发此回调方法
    /// </summary>
    [Parameter]
    public Func<QueryPageOptions, Task<QueryData<TItem>>>? OnQueryAsync { get; set; }

    /// <summary>
    /// 获得/设置 树形数据节点展开式回调委托方法
    /// </summary>
    [Parameter]
    public Func<TItem, Task<IEnumerable<TableTreeNode<TItem>>>>? OnTreeExpand { get; set; }

    /// <summary>
    /// 获得/设置 生成树状结构回调方法
    /// </summary>
    [Parameter]
    public Func<IEnumerable<TItem>, Task<IEnumerable<TableTreeNode<TItem>>>>? TreeNodeConverter { get; set; }

    /// <summary>
    /// 获得/设置 保存按钮异步回调方法
    /// </summary>
    [Parameter]
    public Func<TItem, ItemChangedType, Task<bool>>? OnSaveAsync { get; set; }

    /// <summary>
    /// 获得/设置 删除按钮异步回调方法
    /// </summary>
    [Parameter]
    public Func<IEnumerable<TItem>, Task<bool>>? OnDeleteAsync { get; set; }


    /// <summary>
    /// 获得/设置 被选中数据集合
    /// </summary>
    [Parameter]
    public List<TItem> SelectedRows { get; set; } = new List<TItem>();

    /// <summary>
    /// 获得/设置 选中行变化回调方法
    /// </summary>
    [Parameter]
    public EventCallback<List<TItem>> SelectedRowsChanged { get; set; }

    /// <summary>
    /// 获得/设置 TableFooter 实例
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<TItem>>? TableFooter { get; set; }

    /// <summary>
    /// 获得/设置 是否显示表脚 默认为 false
    /// </summary>
    [Parameter]
    public bool ShowFooter { get; set; }

    /// <summary>
    /// 获得/设置 Table Footer 模板
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<TItem>>? FooterTemplate { get; set; }

    /// <summary>
    /// 编辑框的大小
    /// </summary>
    [Parameter]
    public Size EditDialogSize { get; set; } = Size.ExtraExtraLarge;

    /// <summary>
    /// 获得/设置 保存删除后回调委托方法
    /// </summary>
    [Parameter]
    public Func<Task>? OnAfterModifyAsync { get; set; }

    /// <summary>
    /// 选中行变化事件
    /// </summary>
    /// <param name="items"></param>
    public void OnSelectedRowsChanged(List<TItem> items)
    {
        if (IsRadio == true && items.Count > 0)
        {
            items = [items.Last()];
            SelectedRows = items;
        }

        if (SelectedRowsChanged.HasDelegate)
            SelectedRowsChanged.InvokeAsync(items);
        OnSelectRowsAsync?.Invoke(items);
    }

    /// <summary>
    /// 获得/设置 选中行变化方法
    /// </summary>
    [Parameter]
    public Func<List<TItem>, Task>? OnSelectRowsAsync { get; set; }

    /// <summary>
    /// 获得/设置 是否显示行内扩展编辑按钮 默认为 null 未设置时使用
    /// </summary>
    [Parameter]
    public Func<TItem, bool>? ShowExtendEditButtonCallback { get; set; }

    /// <summary>
    /// 获得/设置 是否显示行内扩展删除按钮 默认为 null 未设置时使用
    /// </summary>
    [Parameter]
    public Func<TItem, bool>? ShowExtendDeleteButtonCallback { get; set; }

    /// <summary>
    /// 获取/设置 明细行模板
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? DetailRowTemplate { get; set; }

    /// <summary>
    /// 获取/设置 行单击事件委托回调
    /// </summary>
    [Parameter]
    public Func<TItem, Task>? OnClickRowCallback { get; set; }

    /// <summary>
    /// 获得/设置 是否显示行号列 默认为 false
    /// </summary>
    [Parameter]
    public bool ShowLineNo { get; set; }

    /// <summary>
    /// 获得/设置 双击行回调委托方法
    /// </summary>
    [Parameter]
    public Func<TItem, Task>? OnDoubleClickRowCallback { get; set; }


    [NotNull]
    private Table<TItem>? Instance { get; set; }

    /// <summary>
    /// 获取表实体
    /// </summary>
    public Table<TItem> Table => Instance;

    /// <summary>
    /// 获得 当前表格所有 Rows 集合
    /// </summary>
    public List<TItem> Rows => Instance.Rows;

    /// <summary>
    /// 显示/隐藏 Loading 遮罩
    /// true 时显示，false 时隐藏
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ValueTask ToggleLoading(bool v) => Instance.ToggleLoading(v);

    /// <summary>
    /// 查询按钮调用此方法 参数 pageIndex 默认值 null 保持上次页码 第一页页码为 1
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task QueryAsync() => Instance.QueryAsync();

    /// <summary>
    /// 服务器 列数据持久化存储解决方案  填写名称即可，多个表不要重复，服务器会覆盖 自动按照用户存储
    /// </summary>
    [Parameter]
    public string ColumnPersistenceStoreName { get; set; }

    /// <summary>
    /// 位置拖拽事件
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    private async Task OnDragColumnEndAsync(string columnName, IEnumerable<ITableColumn> columns)
    {
        await ColumnChangedPersistenceStore(columns);
    }

    /// <summary>
    /// 列宽度改变事件
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    private async Task OnResizeColumnAsync(string columnName, float width)
    {
        var col = Instance.Columns.FirstOrDefault(t => t.GetFieldName() == columnName);
        if (col == null) return;
        col.Width = (int)width;
        await ColumnChangedPersistenceStore(Instance.Columns);
    }

    /// <summary>
    /// 列改变隐藏/显示状态回调方法
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="visible"></param>
    /// <returns></returns>
    private async Task OnColumnVisibleChanged(string columnName, bool visible)
    {
        var col = Instance.Columns.FirstOrDefault(t => t.GetFieldName() == columnName);
        if (col == null) return;
        col.Visible = visible;
        await ColumnChangedPersistenceStore(Instance.Columns);
    }

    class PersistenceStoreColumn
    {
        public string Name { get; set; }
        public string? Text { get; set; }
        public bool? Visible { get; set; }
        public int? Width { get; set; }
        public int Order { get; set; }
    }

    static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    /// <summary>
    /// 客户数据持久化存储在服务器中，自动按照用户存储
    /// </summary>
    /// <param name="columns"></param>
    /// <returns></returns>
    private async Task ColumnChangedPersistenceStore(IEnumerable<ITableColumn> columns)
    {
        if (string.IsNullOrWhiteSpace(ColumnPersistenceStoreName)) return;

        var lis = new List<object>();

        int index = 1;
        foreach (var item in columns)
        {
            var li = new PersistenceStoreColumn()
            {
                Name = item.GetFieldName(),
                Text = item.Text,
                Visible = item.Visible,
                Width = item.Width,
                Order = index++
            };
            lis.Add(li);
        }
        var col = JsonSerializer.Serialize(lis, jsonSerializerOptions);

        db.Storageable(new SysUserLocalStorageData()
        {
            TableName = ColumnPersistenceStoreName,
            UserName = AdminService.UserName,
            Value = col
        }).WhereColumns(t => new { t.TableName, t.UserName }).ExecuteCommand();
        cache.Remove<SysUserLocalStorageData>();
    }

    /// <summary>
    /// 列创建时回调事件
    /// </summary>
    /// <param name="tableColumns"></param>
    /// <returns></returns>
    private async Task OnColumnCreating(List<ITableColumn> tableColumns)
    {
        if (string.IsNullOrWhiteSpace(ColumnPersistenceStoreName)) return;

        try
        {
            var storageData = //db.Queryable<SysUserLocalStorageData>()
                cache.GetAll<SysUserLocalStorageData>()
                .Where(t => t.TableName == ColumnPersistenceStoreName && t.UserName == AdminService.UserName)
                .First();

            if (storageData == null || string.IsNullOrWhiteSpace(storageData?.Value)) return;

            var cols = JsonDocument.Parse(storageData.Value).Deserialize<List<PersistenceStoreColumn>>();

            if (cols == null || cols.Count == 0) return;

            var columns = new List<ITableColumn>();
            foreach (var item in tableColumns)
            {
                var col = cols.FirstOrDefault(it => it.Name == item.GetFieldName() && it.Text == item.Text);
                if (col != null)
                {
                    item.Order = col.Order;
                    item.Width = col.Width;
                    item.Visible = col.Visible;
                }
                columns.Add(item);
            }
            Instance.Columns.Clear();
            Instance.Columns.AddRange(columns.OrderBy(it => it.Order));
        }
        catch { }
    }

    [NotNull]
    [Inject]
    private ISqlSugarClient? db { get; set; }

    [NotNull]
    [Inject]
    private ICache? cache { get; set; }

    [Inject]
    [NotNull]
    private IAdmin? AdminService { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }


    /// <summary>
    /// 权限按钮获取
    /// </summary>
    /// <param name="operate"></param>
    /// <returns></returns>
    private bool AuthorizeButton(string operate)
    {
        var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        return AdminService.AuthorizingBlock(AdminService.UserName, url, operate);
    }

    /// <summary>
    /// 是否单选
    /// </summary>
    [Parameter]
    public bool IsRadio { get; set; }
}
