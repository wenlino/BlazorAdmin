namespace AdminSenyun.Models;

/// <summary>
/// 假删除接口过滤器
/// </summary>
internal interface IDeletedFilter
{
    /// <summary>
    /// 软删除
    /// </summary>
    bool IsDelete { get; set; }
}