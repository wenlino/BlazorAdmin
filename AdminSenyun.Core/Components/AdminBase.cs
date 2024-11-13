using AdminSenyun.Core.Extensions;
using AdminSenyun.Data.Core;
using AdminSenyun.Data.Service;
using AdminSenyun.Models;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SqlSugar;
using Stimulsoft.Report.Web;


namespace AdminSenyun.Core.Components;

public class AdminBase : BootstrapModuleComponentBase
{
    #region 注入服务

    /// <summary>
    /// 数据库
    /// </summary>
    [Inject]
    [NotNull]
    public SqlSugar.ISqlSugarClient? Db { get; set; }

    /// <summary>
    /// 权限管理服务
    /// </summary>
    [Inject]
    [NotNull]
    public IAdmin? AdminService { get; set; }

    /// <summary>
    /// 提供用于查询和管理URI导航服务
    /// </summary>
    [Inject]
    [NotNull]
    public NavigationManager? NavigationManager { get; set; }

    /// <summary>
    /// Dialog组件服务(弹窗服务)
    /// </summary>
    [Inject]
    [NotNull]
    public DialogService? DialogService { get; set; }

    /// <summary>
    /// Toast组件服务(轻量弹窗服务)
    /// </summary>
    [Inject]
    [NotNull]
    public ToastService? ToastService { get; set; }

    /// <summary>
    /// Message组件服务(消息弹出服务)
    /// </summary>
    [Inject]
    [NotNull]
    public MessageService? MessageService { get; set; }

    /// <summary>
    /// 提供用于查询和管理URI导航的抽象
    /// </summary>
    [Inject]
    [NotNull]
    public NavigationManager? Navigation { get; set; }

    /// <summary>
    /// 文件下载服务类
    /// </summary>
    [Inject]
    [NotNull]
    public DownloadService DownloadService { get; set; }

    /// <summary>
    /// 设置服务
    /// </summary>
    [Inject]
    [NotNull]
    public ISysSetting SysSetting { get; set; }

    /// <summary>
    /// 模态框弹窗服务
    /// </summary>
    [Inject]
    [NotNull]
    public SwalService SwalService { get; set; }

    /// <summary>
    /// WinBox弹窗服务
    /// </summary>
    [Inject]
    [NotNull]
    public WinBoxService? WinBoxService { get; set; }

    /// <summary>
    /// 报表服务
    /// </summary>
    [Inject]
    [NotNull]
    public RepService? RepService { get; set; }

    #endregion

    /// <summary>
    /// 按钮权限获取
    /// </summary>
    /// <param name="operate"></param>
    /// <returns></returns>
    public bool AuthorizeButton(string operate)
    {
        var url = NavigationManager?.ToBaseRelativePath(NavigationManager.Uri);
        return AdminService.AuthorizingBlock(AdminService.UserName, url, operate);
    }

    #region 消息框


    /// <summary>
    /// 成功消息
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task MsgSuccess(string text) => await Msg(text, Color.Success);

    /// <summary>
    /// 错误消息
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task MsgError(string text) => await Msg(text, Color.Danger);

    /// <summary>
    /// 警告消息
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task MsgWarning(string text) => await Msg(text, Color.Warning);

    /// <summary>
    /// 弹出消息
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public async Task Msg(string text, Color color = Color.Primary)
    {
        var icon = color == Color.Success ? "fas fa-face-smile" : //成功笑脸
        color == Color.Warning ? "fas fa-face-meh" ://警告 平
        color == Color.Danger ? "fas fa-face-frown" ://失败 哭
        "fa-solid fa-circle-info";//消息

        await MessageService.Show(new MessageOption()
        {
            Content = text,
            Icon = "fa-solid fa-circle-info",
            Color = color
        });
    }

    #endregion

    #region 弹窗对话框


    /// <summary>
    /// 弹出消息对话框
    /// </summary>
    /// <typeparam name="TCom"></typeparam>
    /// <param name="body"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public async Task ShowDialog<TCom>(object? body = null, string? title = null, Size size = Size.Large, bool full = false, Action<object>? action = null, Action<DictionaryEx<TCom>>? parameters = null) where TCom : class, IComponent, new()
    {
        var par = new DictionaryEx<TCom>();
        parameters?.Invoke(par);
        var pars = par.Build();
        var option = new DialogOption()
        {
            ShowMaximizeButton = true,
            Component = BootstrapDynamicComponent.CreateComponent<TCom>(pars),
            BodyContext = body,
            ShowFooter = false,
            Size = size,
            Title = title ?? "消息对话框",
            FullScreenSize = full ? FullScreenSize.Always : FullScreenSize.None,
            OnCloseAsync = () =>
            {
                if (body != null)
                {
                    action?.Invoke(body);
                    StateHasChanged();
                }
                return Task.CompletedTask;
            },
        };
        await DialogService.Show(option);
    }

    public async Task ShowMessageDialog(string message, string title = "提醒")
    {
        var option = new DialogOption()
        {
            ShowMaximizeButton = true,
            Title = title,
            Size = Size.ExtraLarge,
            BodyTemplate = b =>
            {
                b.OpenElement(0, "pre");
                b.AddAttribute(1, "style", "max-height:600px;max-width:100%");
                b.AddContent(1, message);
                b.CloseComponent();
            }
        };
        await DialogService.Show(option);
    }

    public async Task ShowDialog(string url, string title = "消息")
    {
        var option = new DialogOption()
        {
            ShowMaximizeButton = true,
            Title = title,
            Size = Size.ExtraLarge,
            BodyTemplate = b =>
            {
                b.OpenElement(0, "iframe");
                b.AddAttribute(1, "src", url);
                b.AddAttribute(1, "style", "height:600px;width:100%");
                b.CloseComponent();
            }
        };
        await DialogService.Show(option);
    }

    public async Task<bool> ShowModalDialog<TCom>(object? body = null, string? title = null, Size size = Size.ExtraLarge, bool full = false, bool showFooter = true, Func<Task>? onCloseAsync = null, Dictionary<string, object>? parameters = null) where TCom : IComponent, IResultDialog
    {
        var option = new ResultDialogOption()
        {
            ShowMaximizeButton = true,
            BodyContext = body,
            Component = BootstrapDynamicComponent.CreateComponent<TCom>(),
            ShowFooter = showFooter,
            Size = size,
            Title = title ?? "消息对话框",
            FullScreenSize = full ? FullScreenSize.Always : FullScreenSize.None,
            OnCloseAsync = onCloseAsync,
            ComponentParameters = parameters
        };
        var result = await DialogService.ShowModal<TCom>(option);
        return result == DialogResult.Yes;
    }

    public async Task<bool> ShowResultDialog(string message, string title = "消息", Size size = Size.Medium)
    {
        var option = new ResultDialogOption()
        {
            Size = size,
            Title = title,
            ComponentParameters = new Dictionary<string, object>()
            {
                ["Message"] = message
            },
        };
        var result = await DialogService.ShowModal<ResultDialog>(option);
        if (result == DialogResult.Yes) { return true; }
        else { return false; }
    }


    public async Task<(bool Result, string Message)> ShowInputDialog(string text = "", string title = "请输入")
    {
        var option = new ResultDialogOption()
        {
            Title = title,
            Size = Size.ExtraSmall,
            BodyTemplate = builder =>
            {
                builder.OpenComponent<BootstrapInput<string>>(0);
                builder.AddAttribute(1, nameof(BootstrapInput<string>.Value), text);
                builder.AddAttribute(1, nameof(BootstrapInput<string>.ValueChanged),
                    EventCallback.Factory.Create<string>(this, t => text = t));
                builder.CloseComponent();
            },
            IsKeyboard = false,
        };
        var result = await DialogService.ShowModal<ResultDialog>(option);
        return (result == DialogResult.Yes, text);
    }

    #endregion

    #region 模态对话框

    /// <summary>
    /// 返回成功模态框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public async Task<bool> SwalSuccess(string text, string? title = null) =>
        await Swal(text, title, SwalCategory.Success);

    /// <summary>
    /// 返回错误模态框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public async Task<bool> SwalError(string text, string? title = null) =>
        await Swal(text, title, SwalCategory.Error);

    /// <summary>
    /// 返回警告模态框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public async Task<bool> SwalWarning(string text, string? title = null) =>
        await Swal(text, title, SwalCategory.Warning);

    /// <summary>
    /// 返回提示模态框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public async Task<bool> SwalInformation(string text, string? title = null) =>
        await Swal(text, title, SwalCategory.Information);

    /// <summary>
    /// 返回疑问模态框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public async Task<bool> SwalQuestion(string text, string? title = null) =>
        await Swal(text, title, SwalCategory.Question);

    /// <summary>
    /// 返回模态框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <param name="swalCategory"></param>
    /// <returns></returns>
    public async Task<bool> Swal(string text, string title, SwalCategory swalCategory)
    {
        var op = new SwalOption()
        {
            Title = title,
            Content = text,
            Category = swalCategory,
        };
        var ret = await SwalService.ShowModal(op);
        return ret;
    }

    #endregion

    #region WinBox弹窗

    public async Task WinBox<TComponent>(string? title = null,
        Action<DictionaryEx<TComponent>>? parameters = null,
        Action<WinBoxOption>? option = null)
        where TComponent : ComponentBase, new()
    {
        var op = new WinBoxOption()
        {
            Index = 10,
            Top = "135px",
            Left = "300px",
            Right = "10px",
            Bottom = "40px",
            Width="750px",
            //Bottom = "10px",
            //Class = "bb-win-box",
            MinHeight = 500,
            Border = 2,
            //Background = "var(--bb-primary-color)",
        };
        option?.Invoke(op);
        op.Title = title ?? "信息窗";

        var dx = new DictionaryEx<TComponent>();
        parameters?.Invoke(dx);

        op.ContentTemplate = BootstrapDynamicComponent.CreateComponent(typeof(TComponent), dx.Build()).Render();

        await WinBoxService.Show(op);
    }

    #endregion  

    /// <summary>
    /// Items获取table树结构tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public IEnumerable<TableTreeNode<T>> BuildTableTreeNodes<T>(IEnumerable<T> items, long parentId = 0) where T : EntityBaseTreeId
    {
        var ret = new List<TableTreeNode<T>>();
        ret.AddRange(items.Where(i => i.ParentId == parentId).Select((foo, index) => new TableTreeNode<T>(foo)
        {
            HasChildren = items.Any(i => i.ParentId == foo.Id),
            // 如果子项集合有值 则默认展开此节点
            IsExpand = items.Any(i => i.ParentId == foo.Id),
            // 获得子项集合
            Items = BuildTableTreeNodes(items, foo.Id)
        }));
        return ret;
    }


    /// <summary>
    /// 获取树结构list绑定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="textColName"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public List<TreeViewItem<long>> BuildTreeItems<T>(IEnumerable<T> items, string textColName = "Text", long parentId = 0) where T : EntityBaseTreeId
    {
        var ret = new List<TreeViewItem<long>>();
        ret.AddRange(items.Where(i => i.ParentId == parentId).Select((foo, index) => new TreeViewItem<long>(foo.Id)
        {
            Text = foo.GetType().GetProperty(textColName)?.GetValue(foo)?.ToString(),
            // 获得子项集合
            Items = BuildTreeItems(items, textColName, foo.Id)
        }));
        return ret;
    }

    /// <summary>
    /// 树结构类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private class TableTreeItem<T> where T : class
    {
        public string? Code { get; set; }
        public string? ParentCode { get; set; }
        public string? Text { get; set; }
        public T Item { get; set; }
    }

    /// <summary>
    /// 获取表格树结构
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="items">数据源 </param>
    /// <param name="idColName">id字段列名称</param>
    /// <param name="pColName">父项字段列名称</param>
    /// <param name="parent">root层数据</param>
    /// <returns>表格树结构</returns>
    public List<TableTreeNode<T>> BuildTableTreeNodes<T>(IEnumerable<T> items, string idColName, string pColName, string? parent = null) where T : class
    {
        var ts = items.Select(t => new TableTreeItem<T>()
        {
            Code = t.GetType().GetProperty(idColName)?.GetValue(t)?.ToString(),
            ParentCode = t.GetType().GetProperty(pColName)?.GetValue(t)?.ToString(),
            Item = t
        });

        var ret = BuildTableTreeNodes(ts, parent);

        return ret;
    }

    /// <summary>
    /// 获取表格树结构
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="items">数据源 </param>
    /// <param name="idColName">id字段列名称</param>
    /// <param name="pColName">父项字段列名称</param>
    /// <param name="parent">root层数据</param>
    /// <returns>表格树结构</returns>
    private List<TableTreeNode<T>> BuildTableTreeNodes<T>(IEnumerable<TableTreeItem<T>> items, string? parent = null) where T : class
    {
        var ret = new List<TableTreeNode<T>>();

        var ts = items.Where(i => i.ParentCode == parent);

        var subs = ts.Select((f, index) => new TableTreeNode<T>(f.Item)
        {
            HasChildren = items.Any(i => i.ParentCode == f.Code),
            // 如果子项集合有值 则默认展开此节点
            IsExpand = items.Any(i => i.ParentCode == f.Code),
            // 获得子项集合
            Items = BuildTableTreeNodes(items, f.Code)
        });

        ret.AddRange(subs);
        return ret;
    }


    /// <summary>
    /// 获取文本树结构
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="items">数据源 </param>
    /// <param name="idColName">id字段列名称</param>
    /// <param name="pColName">父项字段列名称</param>
    /// <param name="textColName">文本列</param>
    /// <param name="parent">root层数据</param>
    /// <returns>表格树结构</returns>
    public List<TreeViewItem<T>> BuildTreeItems<T>(IEnumerable<T> items, string idColName, string pColName, string textColName = "Text", string? parent = null) where T : class
    {
        var ts = items.Select(t => new TableTreeItem<T>()
        {
            Code = t.GetType().GetProperty(idColName)?.GetValue(t)?.ToString(),
            ParentCode = t.GetType().GetProperty(pColName)?.GetValue(t)?.ToString(),
            Text = t.GetType().GetProperty(textColName)?.GetValue(t)?.ToString(),
            Item = t
        });

        var ret = BuildTreeItems(ts, idColName, pColName, parent);

        return ret;
    }

    /// <summary>
    /// 获取文本树结构
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="items">数据源 </param>
    /// <param name="idColumnName">id字段列名称</param>
    /// <param name="pColumnName">父项字段列名称</param>
    /// <param name="parent">root层数据</param>
    /// <returns>表格树结构</returns>
    private List<TreeViewItem<T>> BuildTreeItems<T>(IEnumerable<TableTreeItem<T>> items, string idColumnName, string pColumnName, string? parent = null) where T : class
    {
        var ret = new List<TreeViewItem<T>>();

        var ts = items.Where(i => i.ParentCode == parent);

        var subs = ts.Select((f, index) => new TreeViewItem<T>(f.Item)
        {
            Text = f.Text,
            Items = BuildTreeItems(items, idColumnName, pColumnName, f.Code)
        });

        ret.AddRange(subs);
        return ret;
    }


    /// <summary>
    /// 按照名称获取系统中的设置内容
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string? GetSysSetting(string name)
    {
        return SysSetting[name]?.Value;
    }

    /// <summary>
    /// 按照名称获取设置中的数据库连接信息
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ISqlSugarClient GetSysSettingISqlSugar(string name) => SysSetting.GetSqlSugarClient(name);


    /// <summary>
    /// 获取系统设置上文文件保存根目录
    /// </summary>
    /// <returns></returns>
    public string GetSysFileRootPath() => GetSysSetting("SysFileRootPath") ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadFile");


    /// <summary>
    /// 显示上传文件窗口
    /// </summary>
    /// <returns></returns>
    public async Task SysFileUpload(bool isMultiple = true, Func<long, Task>? onIdChanged = null,
        Func<SysFile, Task>? onSysFileChanged = null,
        Func<Task>? close = null,
        Func<long, Task<bool>>? onDelete = null)
    {
        var option = new DialogOption()
        {
            ShowMaximizeButton = true,
            Component = BootstrapDynamicComponent.CreateComponent<UploadSysFile>(
                new DictionaryEx<UploadSysFile>()
                .Set(t => t.OnIdChanged, onIdChanged)
                .Set(t => t.OnSysFileChanged, onSysFileChanged)
                .Set(t => t.IsMultiple, isMultiple)
                .Set(t => t.OnDelete, onDelete)
                .Build()),
            ShowFooter = false,
            Size = isMultiple ? Size.ExtraLarge : Size.Small,
            Title = "上传文件",
            OnCloseAsync = () =>
            {
                close?.Invoke();
                StateHasChanged();
                return Task.CompletedTask;
            },
        };
        await DialogService.Show(option);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="fileid"></param>
    /// <returns></returns>
    public async Task SysFileDownload(long fileid)
    {
        var file = this.Db.Queryable<SysFile>().Where(t => t.Id == fileid).First();
        var path = Path.Combine(this.GetSysFileRootPath(), file.Folder, file.GetSaveFileName());
        var bytes = File.ReadAllBytes(path);
        await DownloadService.DownloadFromByteArrayAsync(file.GetFileName(), bytes);
    }

    /// <summary>
    /// 预览文件
    /// </summary>
    /// <param name="fileid"></param>
    /// <returns></returns>
    public async Task SysFileView(long fileid)
    {
        var file = this.Db.Queryable<SysFile>().Where(t => t.Id == fileid).First();
        await ShowDialog(file.GetUrl());
    }

    #region 数据处理

    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync<TModel>(IEnumerable<TModel> models) where TModel : class, new()
    {
        try
        {
            Db.Deleteable<TModel>(models).ExecuteCommand();
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    public async Task<bool> SaveAsync<TModel>(TModel model, ItemChangedType changedType) where TModel : class, new()
    {
        try
        {
            if (changedType == ItemChangedType.Add)
            {
                Db.Insertable(model).ExecuteCommand();
            }
            else if (changedType == ItemChangedType.Update)
            {
                Db.Updateable(model).ExecuteCommand();
            }
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// 查询数据方法 可增加条件
    /// </summary>
    /// <typeparam name="TModel">实例模型</typeparam>   
    /// <param name="option">条件</param>
    /// <param name="ext">方法</param>
    /// <returns></returns>
    public Task<QueryData<TModel>> QueryAsync<TModel>(QueryPageOptions option, Func<ISugarQueryable<TModel>, ISugarQueryable<TModel>>? ext = null) where TModel : class, new()
    {
        int count = 0;
        var filter = option.ToFilter();

        var isq = Db.Queryable<TModel>()
                      .WhereIF(filter.HasFilters(), filter.GetFilterLambda<TModel>())
                      .OrderByPropertyNameIF(option.SortOrder != SortOrder.Unset, option.SortName, option.SortOrder == SortOrder.Asc ? SqlSugar.OrderByType.Asc : SqlSugar.OrderByType.Desc);

        isq = ext is null ? isq : ext.Invoke(isq);

        var items = isq.ToPageList(option.PageIndex, option.PageItems, ref count);

        var data = new QueryData<TModel>
        {
            IsSorted = option.SortOrder != SortOrder.Unset,
            IsFiltered = option.Filters.Any(),
            IsAdvanceSearch = option.AdvanceSearches.Any(),
            IsSearch = option.Searches.Any() || option.CustomerSearches.Any(),
            Items = items,
            TotalCount = Convert.ToInt32(count)
        };
        return Task.FromResult(data);
    }

    /// <summary>
    /// 表格查询数据
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="option"></param>
    /// <param name="items"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public Task<QueryData<TModel>> QueryAsync<TModel>(QueryPageOptions option, IEnumerable<TModel> items, int count) where TModel : class, new()
    {
        var filter = option.ToFilter();
        var data = new QueryData<TModel>
        {
            IsSorted = option.SortOrder != SortOrder.Unset,
            IsFiltered = option.Filters.Any(),
            IsAdvanceSearch = option.AdvanceSearches.Any(),
            IsSearch = option.Searches.Any() || option.CustomerSearches.Any(),
            Items = items,
            TotalCount = Convert.ToInt32(count)
        };
        return Task.FromResult(data);
    }

    /// <summary>
    /// 表格数据查询
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public Task<QueryData<TModel>> QueryAsync<TModel>(IEnumerable<TModel> items) where TModel : class, new()
    {
        var data = new QueryData<TModel>
        {
            Items = items
        };
        return Task.FromResult(data);
    }

    #endregion

    /// <summary>
    /// 使用表达式返回Dictionar
    /// </summary>y
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static DictionaryEx<T> GetDictionaryEx<T>() where T : class, new() => new();


    #region  报表

    /// <summary>
    /// 获取报表数据
    /// </summary>
    /// <param name="repName"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Stimulsoft.Report.StiReport GetStiReport(string repName, string exportName, object parameter)
    {
        return RepService.GetStiReport(repName, exportName, parameter);
    }

    /// <summary>
    /// 弹出对话框显示报表
    /// </summary>
    /// <param name="stiReport"></param>
    /// <returns></returns>
    public async Task ShowStiReportViewDialog(Stimulsoft.Report.StiReport stiReport)
    {
        await RepService.ShowStiReportViewDialog(this.DialogService, stiReport);
    }

    #endregion
}