using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

/// <summary>
/// 框架实体基类Id
/// </summary>
public abstract class EntityBaseId
{
    /// <summary>
    /// 雪花Id
    /// </summary>
    [Key]
    [SugarColumn(ColumnDescription = "Id", IsPrimaryKey = true, IsIdentity = false)]
    [Display(Name = "ID")]
    [DisplayName("ID")]
    [AutoGenerateColumn(Order = 1, Visible = false, Sortable = true, Align = Alignment.Left)]
    public virtual long Id { get; set; }
}
