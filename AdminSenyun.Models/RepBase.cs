namespace AdminSenyun.Models;

[SugarTable(null, "表单数据")]
public class RepBase : EntityBase
{
    [Description("表名称")]
    public string Name { get; set; } = "";

    [Description("分组名称")]
    public string GroupName { get; set; } = "";

    [Description("注释")]
    public string? Description { get; set; }

    [Description("表单模板")]
    [SugarColumn(Length = -1)]  
    public string Report { get; set; } = "";
}
