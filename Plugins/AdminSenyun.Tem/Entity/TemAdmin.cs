using AdminSenyun.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Tem.Entity;
/// <summary>
/// 自动建表 ，需要版本号高于历史版本号，并且 类  namespace 以 Entity结尾 同时 特性标记 SugarTable
/// </summary>
[SqlSugar.SugarTable(TableDescription = "模板示例")]
[SqlSugarIndex(nameof(Name))]//单列不允许重复
[SqlSugarIndex("Code", nameof(Code), nameof(Title))]//两列一起不允许重复
public class TemAdmin
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
}
