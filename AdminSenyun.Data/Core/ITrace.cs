namespace AdminSenyun.Data.Core;

/// <summary>
/// 
/// </summary>
public interface ITrace
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="trace"></param>
    public void Log(SysTrace trace);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="filter"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageItems"></param>
    /// <param name="sortList"></param>
    /// <returns></returns>
    (IEnumerable<SysTrace> Items, int ItemsCount) GetAll(string? searchText, TraceFilter filter, int pageIndex, int pageItems, List<string> sortList);
}
