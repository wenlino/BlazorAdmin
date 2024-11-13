using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

/// <summary>
/// 树结构框架实体基类
/// </summary>
public abstract class EntityBaseTree : EntityBaseTreeId
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsOnlyIgnoreUpdate = true, IsNullable = true)]
    [Display(Name = "创建时间")]
    public virtual DateTime? CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", IsOnlyIgnoreInsert = true, IsNullable = true)]
    [Display(Name = "更新时间")]
    public virtual DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户名", IsOnlyIgnoreUpdate = true, IsNullable = true)]
    [Display(Name = "创建者用户名")]
    public virtual string? CreateUserName { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    [SugarColumn(ColumnDescription = "修改者用户名", IsOnlyIgnoreInsert = true, IsNullable = true)]
    [Display(Name = "修改者用户名")]
    public virtual string? UpdateUserName { get; set; }
}