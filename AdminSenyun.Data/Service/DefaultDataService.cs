using BootstrapBlazor.Components;

namespace AdminSenyun.Data.Service;

class DefaultDataService<TModel>(ISqlSugarClient db) : DataServiceBase<TModel> where TModel : class, new()
{
    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override async Task<bool> DeleteAsync(IEnumerable<TModel> models)
    {
        try
        {
            db.Deleteable<TModel>(models).ExecuteCommand();
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
    public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
    {
        try
        {
            if (changedType == ItemChangedType.Add)
            {
                db.Insertable(model).ExecuteCommand();
            }
            else if (changedType == ItemChangedType.Update)
            {
                db.Updateable(model).ExecuteCommand();
            }
            return true;
        }
        catch { return false; }
    }

    public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
    {
        int count = 0;
        var filter = option.ToFilter();
        var items = db.Queryable<TModel>()
                      .WhereIF(filter.HasFilters(), filter.GetFilterLambda<TModel>())
                      .OrderByPropertyNameIF(option.SortOrder != SortOrder.Unset, option.SortName,
                      option.SortOrder == SortOrder.Asc ? OrderByType.Asc : OrderByType.Desc)
                      .ToPageList(option.PageIndex, option.PageItems, ref count);
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
}