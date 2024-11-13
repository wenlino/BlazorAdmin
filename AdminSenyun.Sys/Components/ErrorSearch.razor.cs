using AdminSenyun.Sys.Models;

namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class ErrorSearch
{
    private List<SelectedItem>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public ErrorSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<ErrorSearchModel> ValueChanged { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Items = new List<SelectedItem>
        {
            new SelectedItem("", "全部")
        };
    }
}
