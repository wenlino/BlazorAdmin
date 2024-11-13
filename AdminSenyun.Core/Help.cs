using AdminSenyun.Core.Components;
using AdminSenyun.Models;
using BootstrapBlazor.Components;
using Mapster;
using Microsoft.AspNetCore.Components;
using SqlSugar;

namespace AdminSenyun.Core;

public static class HelpExtensions
{
    /// <summary>
    /// Invoke 拓展 当为null时候不执行，await 避免null报错 
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public static async Task InvokeAwait(this Func<Task>? func)
    {
        if (func != null)
            await func.Invoke();
    }

    /// <summary>
    /// Invoke 拓展 当为null时候不执行，await 避免null报错 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static async Task InvokeAwait<T>(this Func<T, Task>? func, T arg)
    {
        if (func != null)
            await func.Invoke(arg);
    }

    /// <summary>
    /// Invoke 拓展 当为null时候不执行，await 避免null报错 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static async Task InvokeAwait<T, TResult>(this Func<T, Task<TResult>>? func, T arg)
    {
        if (func != null)
            await func.Invoke(arg);
    }


    public static TDestination Adap<TDestination>(this object? source, Action<TDestination>? action = null)
    {
        if (source is IEnumerable<TDestination> lis)
        {
            var d = lis.FirstOrDefault().Adapt<TDestination>();
            action?.Invoke(d);
            return d;
        }
        else
        {
            var d = source.Adapt<TDestination>();
            action?.Invoke(d);
            return d;
        }
    }


    /// <summary>
    /// Items获取table树结构tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public static IEnumerable<TableTreeNode<T>> BuildTableTreeNodes<T>(this IEnumerable<T> items, long parentId = 0) where T : EntityBaseTreeId
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
}


public static class HelpDialogExtensions
{
    public static async Task<(bool Result, string Message)> ShowInputDialog(this DialogService dialogService, string text = "", string title = "请输入")
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
                    EventCallback.Factory.Create<string>(null, t => text = t));
                builder.CloseComponent();
            }
        };
        var result = await dialogService.ShowModal<ResultDialog>(option);
        return (result == DialogResult.Yes, text);
    }

}

public static class HelpDataExtensions
{
    public static Task<QueryData<TModel>> QueryAsync<TModel>(this ISqlSugarClient db, QueryPageOptions option,
        Func<ISugarQueryable<TModel>, ISugarQueryable<TModel>>? ext = null) where TModel : class, new()
    {
        int count = 0;
        var filter = option.ToFilter();

        var isq = db.Queryable<TModel>()
            .WhereIF(filter.HasFilters(), filter.GetFilterLambda<TModel>())
            .OrderByPropertyNameIF(option.SortOrder != SortOrder.Unset, option.SortName, option.SortOrder == SortOrder.Asc ? SqlSugar.OrderByType.Asc : SqlSugar.OrderByType.Desc);

        isq = ext is null ? isq : ext.Invoke(isq);

        var items = isq.ToPageList(option.PageIndex, option.PageItems, ref count);

        var data = new QueryData<TModel>
        {
            IsSorted = option.SortOrder != SortOrder.Unset,
            IsFiltered = option.Filters.Count != 0,
            IsAdvanceSearch = option.AdvanceSearches.Count != 0,
            IsSearch = option.Searches.Count != 0 || option.CustomerSearches.Count != 0,
            Items = items,
            TotalCount = Convert.ToInt32(count)
        };
        return Task.FromResult(data);
    }
}

public static class HelpCoreExtensions
{

}

public static class HelpFileExtensions
{
    public static async Task<bool> SaveFileAsync(this UploadFile upload, string fileName)
    {
        return await upload.SaveToFileAsync(fileName, 500 * 1024 * 1024);
    }
}