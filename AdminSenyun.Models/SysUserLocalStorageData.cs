using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminSenyun.Models;

[SugarTable(null, "用户设置缓存")]
public class SysUserLocalStorageData
{
    /// <summary>
    /// 获得/设置 Id
    /// </summary>
    [Key]
    [Description("Id")]
    public long Id { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    [Description("用户名")]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 Name
    /// </summary>
    [Description("表名称")]
    public string? TableName { get; set; }

    /// <summary>
    /// 获得/设置 Name
    /// </summary>
    [Description("内容")]
    [StringLength(int.MaxValue)]
    public string? Value { get; set; }
}
