using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminSenyun.Models;

/// <summary>
/// 字典配置项
/// </summary>
[SugarTable(null, "字典表")]
public class SysDict
{
    /// <summary>
    /// 获得/设置 ID
    /// </summary>
    [Description("ID")]
    [Key]
    public long Id { get; set; }
    /// <summary>
    /// 获得/设置 字典标签
    /// </summary>
    [Required(ErrorMessage = "{0}不可为空")]
    [Description("字典标签")]
    public string? Category { get; set; }

    /// <summary>
    /// 获得/设置 字典名称
    /// </summary>
    [Required(ErrorMessage = "{0}不可为空")]
    [Description("字典名称")]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 获得/设置 字典字典值
    /// </summary>
    [Required(ErrorMessage = "{0}不可为空")]
    [Description("字典代码")]
    [NotNull]
    public string? Code { get; set; }

    [Description("字典颜色")]
    public Color Color { get; set; }
}
