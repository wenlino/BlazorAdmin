namespace AdminSenyun.Sys.Extensions;

/// <summary>
/// 
/// </summary>
public static class TreeItemExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="navigations"></param>
    /// <param name="selectedItems"></param>
    /// <param name="render"></param>
    /// <param name="parentId"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static List<TreeViewItem<SysNavigation>> ToTreeItemList(this IEnumerable<SysNavigation> navigations, List<long> selectedItems, RenderFragment<SysNavigation> render, long parentId = 0, TreeViewItem<SysNavigation>? parent = null)
    {
        var trees = new List<TreeViewItem<SysNavigation>>();
        var roots = navigations.Where(i => i.ParentId == parentId).OrderBy(i => i.Order);
        foreach (var node in roots)
        {
            var item = new TreeViewItem<SysNavigation>(node)
            {
                Text = node.Name,
                Icon = node.Icon,
                IsActive = selectedItems.Any(v => node.Id == v),
                Parent = parent,
                Template = render,
                CheckedState = selectedItems.Any(i => i == node.Id) ? CheckboxState.Checked : CheckboxState.UnChecked
            };
            item.Items = navigations.ToTreeItemList(selectedItems, render, node.Id, item);
            trees.Add(item);
        }
        return trees;
    }
}
