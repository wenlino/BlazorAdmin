namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class MenuEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public SysNavigation? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? ParementMenus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? Targets { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    private Task OnToggleIconDialog() => DialogService.Show(new DialogOption()
    {
        Title = "选择图标",
        ShowFooter = false,
        Component = BootstrapDynamicComponent.CreateComponent<MenuIconList>(new Dictionary<string, object?>()
        {
            [nameof(MenuIconList.Value)] = Value.Icon,
            [nameof(MenuIconList.ValueChanged)] = EventCallback.Factory.Create<string?>(this, v => Value.Icon = v)
        })
    });

    private Task OnClearIcon()
    {
        Value.Icon = null;
        return Task.CompletedTask;
    }
}
