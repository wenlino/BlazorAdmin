using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

[SugarTable(null, "系统版本")]
public class SysVersion
{
    [Key]
    public long Id { get; set; }
    [Description("名称")]
    public string Name { get; set; }
    [Description("注释")]
    public string Description { get; set; }
    [Description("版本")]
    public string Version { get; set; }
    [Description("更新时间")]
    public DateTime UpDateTime { get; set; }
}
