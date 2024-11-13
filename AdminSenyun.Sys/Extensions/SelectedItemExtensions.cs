namespace AdminSenyun.Sys.Extensions;

/// <summary>
/// 
/// </summary>
public static class SelectedItemExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<SysUser> users) => users.Select(i => new SelectedItem { Value = i.Id!.ToString(), Text = i.ToString() }).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<SysRole> roles) => roles.Select(i => new SelectedItem { Value = i.Id!.ToString(), Text = i.RoleName }).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groups"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<SysGroup> groups) => groups.Select(i => new SelectedItem { Value = i.Id!.ToString(), Text = i.ToString() }).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this Dictionary<string, string> dict) => dict.Select(i => new SelectedItem { Value = i.Key, Text = i.Value }).ToList();
}
