using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Data.Models;
[SugarTable(null, "组权限")]
class SysRoleGroup
{
    [Key]
    public long ID { get; set; }

    public long RoleID { get; set; }

    public long GroupID { get; set; }
}
