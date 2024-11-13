using System.ComponentModel;

namespace AdminSenyun.Models;

[Description("系统设置类型")]
public enum SettingTyp
{
    [Description("默认")]
    None = 0,
    [Description("字符串")]
    String = 1,
    [Description("Bool类型")]
    Bool = 2,
    [Description("下拉选项")]
    Down = 3,
    [Description("数字")]
    Number = 4,
    [Description("SqlServer")]
    SqlServer = 1001,
    [Description("Access")]
    Access = 1002,
    [Description("Sqlite")]
    Sqlite = 1003
}
