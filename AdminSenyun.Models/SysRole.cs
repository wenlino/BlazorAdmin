using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

/// <summary>
/// Role 实体类
/// </summary>
[SugarTable(null, "权限")]
public class SysRole
{
    /// <summary>
    /// 获得/设置 角色主键ID
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 获得/设置 角色名称
    /// </summary>
    [DisplayName("角色名称")]
    [NotNull]
    public string? RoleName { get; set; }

    /// <summary>
    /// 获得/设置 角色描述
    /// </summary>
    [DisplayName("角色描述")]
    [NotNull]
    public string? Description { get; set; }
}
