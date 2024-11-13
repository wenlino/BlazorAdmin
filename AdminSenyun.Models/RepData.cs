namespace AdminSenyun.Models;

[SugarTable(null, "数据源详细")]
public class RepData : EntityBase
{
    [Description("表单ID")]
    public long RepBaseId { get; set; }
    [Description("生成结果表名称")]
    public string Name { get; set; } = "";

    [Description("SQL设置中链接表名")]
    public string DbName { get; set; } = "";

    [SugarColumn(Length = -1)]
    [Description("SQL语句")]
    public string Sql { get; set; } = "";

    [SugarColumn(Length = -1, IsNullable = true)]
    [Description("测试数据变量（只在当前测试有效）")]
    public string? Declare { get; set; } = "declare @id nvarchar(50) = '0'";
}
