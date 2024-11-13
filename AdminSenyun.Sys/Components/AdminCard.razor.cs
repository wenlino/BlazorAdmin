namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class AdminCard
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? AuthorizeKey { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public string? HeaderText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Inject]
    [NotNull]
    private IAdmin? AdminService { get; set; }


    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    private Task<bool> OnQueryCondition(string name)
    {
        var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        return Task.FromResult(AdminService.AuthorizingBlock(AdminService.UserName, url, name));
    }
}
