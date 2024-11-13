using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

public class EntityBaseDelFilter : EntityBase, IDeletedFilter
{
    /// <summary>
    /// 软删除
    /// </summary>
    [SugarColumn(ColumnDescription = "软删除")]
    [Display(Name = "软删除")]
    public virtual bool IsDelete { get; set; } = false;
}
