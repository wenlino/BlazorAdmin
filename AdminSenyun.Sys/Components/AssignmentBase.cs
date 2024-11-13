namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TItem"></typeparam>
public abstract class AssignmentBase<TItem> : ComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<TItem>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<long>? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Action<List<long>>? OnValueChanged { get; set; }
}
