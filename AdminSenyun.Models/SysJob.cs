using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

[SugarTable(null, "任务管理表")]
public class SysJob : EntityBaseId
{
    [Description("名称")]
    [StringLength(200)]
    public string Name { get; set; }

    [Description("类型")]
    [StringLength(500)]
    public string Type { get; set; }

    [Description("Cron表达式")]
    [StringLength(50)]
    public string Cron { get; set; }

    [Description("暂停")]
    public bool IsPaused { get; set; }

    [Description("描述")]
    public string Description { get; set; }
}
