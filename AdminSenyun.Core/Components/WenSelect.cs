using AdminSenyun.Data.Core;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace AdminSenyun.Core.Components;

[JSModuleAutoLoader("/_content/BootstrapBlazor/Components/Select/Select.razor.js")]
public class WenSelect<TValue> : Select<TValue>
{
    [Inject, NotNull] protected IDict? dict { get; set; }
    /// <summary>
    /// 字典名称
    /// </summary>
    [Parameter]
    public string? DictCategory { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (DictCategory != null)
        {
            this.Items = dict.GetDicts(DictCategory).Select(t => new SelectedItem(t.Code, t.Name)).ToList();
        }
    }
}
