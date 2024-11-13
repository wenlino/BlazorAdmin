using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Data.Models;

[SugarTable(null, "用户组权限")]
class SysUserGroup
{
    [Key]
    public long ID { get; set; }

    public long UserID { get; set; }

    public long GroupID { get; set; }
}
