using Microsoft.AspNetCore.Components.Web;

namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class MenuTree
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public List<SelectedItem>? Lookup { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public string? DisplayText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public long Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public EventCallback<long> ValueChanged { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    private DialogOption? Option { get; set; }

    private async Task OnSelectMenu()
    {
        Option = new DialogOption()
        {
            IsScrolling = true,
            Title = "选择菜单",
            BodyTemplate = BootstrapDynamicComponent.CreateComponent<ParentMenuTree>(new Dictionary<string, object?>
            {
                [nameof(ParentMenuTree.Value)] = Value,
                [nameof(ParentMenuTree.ValueChanged)] = EventCallback.Factory.Create<long>(this, OnValueChanged)
            }).Render(),
            FooterTemplate = BootstrapDynamicComponent.CreateComponent<Button>(new Dictionary<string, object?>
            {
                [nameof(Button.Color)] = Color.Primary,
                [nameof(Button.Text)] = "确认",
                [nameof(Button.Icon)] = "fa-solid fa-fw fa-check",
                [nameof(Button.OnClick)] = EventCallback.Factory.Create<MouseEventArgs>(this, async () =>
                {
                    if (Option != null)
                    {
                        await Option.CloseDialogAsync();
                    }
                }),
            }).Render()
        };
        await DialogService.Show(Option);
    }

    private Task OnClearText() => OnValueChanged(0);

    private async Task OnValueChanged(long v)
    {
        if (Value != v)
        {
            Value = v;
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }
        }
    }
}
