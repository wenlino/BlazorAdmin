using BootstrapBlazor.Components;

namespace AdminSenyun.Core.Components;

public class AdminBaseResultDialog : AdminBase, IResultDialog
{
    public Func<Task>? OnCloseChanged { get; set; }

    public virtual async Task OnClose(DialogResult result)
    {
        await OnCloseChanged.InvokeAwait();
    }
}
