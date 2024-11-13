using AdminSenyun.Sys.Models;

namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class TraceSearch
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public TraceSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<TraceSearchModel> ValueChanged { get; set; }
}
