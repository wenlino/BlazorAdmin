using Microsoft.AspNetCore.Components.Routing;
using AdminSenyun.Core.Extensions;
using AdminSenyun.Models;
using BootstrapBlazor.Components;
using SqlSugar;

namespace AdminSenyun.Core.Extensions;

/// <summary>
/// 
/// </summary>
public static class MenuExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    public static MenuItem Parse(this SysNavigation menu)
    {
        return new()
        {
            Text = menu.Name,
            Url = menu.Url?.Replace("~", ""),
            Icon = menu.Icon,
            Match = NavLinkMatch.All,
            Target = menu.Target,
            Id = menu.Id.ToString(),
            ParentId = menu.ParentId.ToString()
        };
    }

    /// <summary>
    /// 获得带层次关系的菜单集合
    /// </summary>
    /// <param name="navigations">未层次化菜单集合</param>
    /// <returns>带层次化的菜单集合</returns>
    public static IEnumerable<MenuItem> CascadeMenus(IEnumerable<SysNavigation> navigations)
    {
        var root = navigations.Where(m => m.ParentId.ToString() == "0")
                        .OrderBy(m => m.Order)
                        .Select(m => m.Parse())
                        .ToList();
        CascadeMenus(navigations, root);
        return root;
    }

    private static void CascadeMenus(IEnumerable<SysNavigation> navigations, List<MenuItem> level)
    {
        level.ForEach(m =>
        {
            m.Items = navigations.Where(sub => sub.ParentId.ToString() == m.Id).OrderBy(sub => sub.Order).Select(sub => sub.Parse()).ToList();
            CascadeMenus(navigations, m.Items.ToList());
        });
    }
}
