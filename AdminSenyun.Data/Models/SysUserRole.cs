using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Data.Models;

[SugarTable(null, "用户权限")]
public class SysUserRole
{
    [Key]
    public long ID { get; set; }

    public long UserID { get; set; }

    public long RoleID { get; set; }
}
