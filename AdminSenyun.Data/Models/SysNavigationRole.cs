using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Data.Models;

[SugarTable(null, "菜单权限")]
class SysNavigationRole
{
    [Key]
    public long ID { get; set; }

    public long NavigationID { get; set; }

    public long RoleID { get; set; }
}
