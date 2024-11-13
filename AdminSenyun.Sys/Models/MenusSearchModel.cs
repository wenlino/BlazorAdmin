using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Sys.Models;

/// <summary>
/// 
/// </summary>
public class MenusSearchModel : ITableSearchModel
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "名称")]
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "地址")]
    public string? Url { get; set; }

    /// <summary>
    /// 获得/设置 链接目标
    /// </summary>
    [Display(Name = "目标")]
    public string? Target { get; set; }

    /// <summary>
    /// 获得/设置 是否为资源文件, 0 表示菜单 1 表示资源 2 表示按钮
    /// </summary>
    [Display(Name = "类型")]
    public EnumResource? IsResource { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IFilterAction> GetSearches()
    {
        var ret = new List<IFilterAction>();
        if (!string.IsNullOrEmpty(Name))
        {
            ret.Add(new SearchFilterAction(nameof(SysNavigation.Name), Name, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Url))
        {
            ret.Add(new SearchFilterAction(nameof(SysNavigation.Url), Url, FilterAction.Equal));
        }

        if (IsResource.HasValue)
        {
            ret.Add(new SearchFilterAction(nameof(SysNavigation.IsResource), IsResource.Value, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Target))
        {
            ret.Add(new SearchFilterAction(nameof(SysNavigation.Target), Target, FilterAction.Equal));
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        Name = null;
        Url = null;
        IsResource = null;
        Target = null;
    }
}
