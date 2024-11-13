using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSenyun.Core.Components;

[CascadingTypeParameter(nameof(TItem))]
[JSModuleAutoLoader("./_content/AdminSenyun.Core/Components/AdminTable.razor.js", JSObjectReference = true)]
public class AdminTableDb<TItem> : AdminTable<TItem> where TItem : class, new()
{

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        OnQueryAsync ??= QueryAsync;
        OnSaveAsync ??= SaveAsync;
        OnDeleteAsync ??= DeleteAsync;
    }

    [NotNull]
    [Parameter]
    public ISqlSugarClient? Db { get; set; }


    Task<QueryData<TItem>> QueryAsync(QueryPageOptions option)
    {
        int count = 0;
        var filter = option.ToFilter();
        var items = Db.Queryable<TItem>()
                      .WhereIF(filter.HasFilters(), filter.GetFilterLambda<TItem>())
                      .OrderByPropertyNameIF(option.SortOrder != SortOrder.Unset, option.SortName,
                      option.SortOrder == SortOrder.Asc ? OrderByType.Asc : OrderByType.Desc)
                      .ToPageList(option.PageIndex, option.PageItems, ref count);
        var data = new QueryData<TItem>
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


    Task<bool> SaveAsync(TItem model, ItemChangedType changedType)
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
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    Task<bool> DeleteAsync(IEnumerable<TItem> models)
    {
        try
        {
            Db.Deleteable<TItem>(models).ExecuteCommand();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }
}
