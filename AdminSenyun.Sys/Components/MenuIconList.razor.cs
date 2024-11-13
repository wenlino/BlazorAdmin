namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class MenuIconList
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    private async Task OnSelctedIcon()
    {
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
