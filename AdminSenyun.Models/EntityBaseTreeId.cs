using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

/// <summary>
/// 树结构框架实体基类Id
/// </summary>
public abstract class EntityBaseTreeId
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

    /// <summary>
    /// 雪花Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父层ID")]
    [Display(Name = "父层ID")]
    [DisplayName("父层ID")]
    [AutoGenerateColumn(Order = 1, Visible = false, Sortable = true, Align = Alignment.Left)]
    public virtual long ParentId { get; set; }
}
