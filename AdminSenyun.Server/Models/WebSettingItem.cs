using System.ComponentModel;

namespace AdminSenyun.Server.Models;

public class WebStyleSetting
{
    [Description("显示底部状态栏")]
    public bool ShowFooter { get; set; } = true;

    [Description("是否固定底部组件")]
    public bool IsFixedFooter { get; set; } = true;

    [Description("是否固定标题组件")]
    public bool IsFixedHeader { get; set; } = true;

    [Description("显示顶部菜单组件")]
    public bool ShowRibbonTab { get; set; } = true;

    [Description("是否显示多标签页")]
    public bool UseTabSet { get; set; } = false;

    [Description("左侧菜单宽度")]
    public string SideWidth { get; set; } = "0";
}
