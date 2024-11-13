using AdminSenyun.Sys.Extensions;
using AdminSenyun.Sys.Models;
namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class MenusSearch
{
    [NotNull]
    [Inject]
    private IDict? DictService { get; set; }

    private IEnumerable<SelectedItem>? CategoryItems { get; set; }

    private IEnumerable<SelectedItem>? ResourceItems { get; set; }

    private List<SelectedItem>? TargetItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public MenusSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<MenusSearchModel> ValueChanged { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        TargetItems = new List<SelectedItem>()
        {
            new SelectedItem("", "全部")
        };
        ResourceItems = typeof(EnumResource).ToSelectList(new SelectedItem("", "全部"));
    }
}
