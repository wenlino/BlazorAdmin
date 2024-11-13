using AdminSenyun.Data.Core;

namespace AdminSenyun.Data.Service;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class TraceService(ISqlSugarClient db) : ITrace
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="filter"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageItems"></param>
    /// <param name="sortList"></param>
    /// <returns></returns>
    public (IEnumerable<SysTrace> Items, int ItemsCount) GetAll(string? searchText, TraceFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        int count = 0;
        var traces = db.Queryable<SysTrace>()
            .WhereIF(!string.IsNullOrEmpty(searchText), t => t.UserName!.Contains(searchText!) || t.Ip!.Contains(searchText!) || t.RequestUrl!.Contains(searchText!))
            .WhereIF(!string.IsNullOrEmpty(filter.UserName), t => t.UserName == filter.UserName)
            .WhereIF(!string.IsNullOrEmpty(filter.Ip), t => t.Ip == filter.Ip)
            .WhereIF(!string.IsNullOrEmpty(filter.RequestUrl), t => t.RequestUrl == filter.RequestUrl)
            .Where(t => t.LogTime >= filter.Star && t.LogTime <= filter.End)
            .OrderByIF(sortList.Any(), string.Join(", ", sortList))
            .OrderByIF(!sortList.Any(), " LogTime desc")
            .ToPageList(pageIndex, pageItems, ref count);

        return (traces, count);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trace"></param>
    public void Log(SysTrace trace)
    {
        db.Insertable(trace).ExecuteCommand();
    }
}
