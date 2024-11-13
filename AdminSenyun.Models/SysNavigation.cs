using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

/// <summary>
/// Bootstrap Admin 后台管理菜单相关操作实体类
/// </summary>
[SugarTable(null, "菜单表")]
public class SysNavigation
{
    /// <summary>
    /// 获得/设置 主键 ID
    /// </summary>
    [DisplayName("ID")]
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 获得/设置 父层ID
    /// </summary>
    [DisplayName("父层ID")]
    public long ParentId { get; set; }

    /// <summary>
    /// 获得/设置 菜单名称
    /// </summary>
    [Display(Name = "名称")]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 获得/设置 菜单序号
    /// </summary>
    [Display(Name = "序号")]
    public int Order { get; set; }

    /// <summary>
    /// 获得/设置 菜单图标
    /// </summary>
    [Display(Name = "图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// 获得/设置 菜单URL地址
    /// </summary>
    [NotNull]
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
    public EnumResource IsResource { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public bool HasChildren { get; set; }
}
