using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;


namespace AdminSenyun.Core.Components;

public class ResultDialog : ComponentBase, IResultDialog
{

    [Parameter]
    public string Message { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
        builder.AddContent(0, Message);
    }

    public async Task OnClose(DialogResult result)
    {

    }
}
