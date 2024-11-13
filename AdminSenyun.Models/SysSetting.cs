namespace AdminSenyun.Models;

[SugarTable(null, "系统设置表")]
[SugarIndex($"Index_{{table}}_{nameof(Name)}", nameof(Name), OrderByType.Asc, true)]
public class SysSetting : EntityBaseTree
{

    [DisplayName("名称")]
    [AutoGenerateColumn(Order = 11, Sortable = true, Filterable = true, Searchable = true, Align = Alignment.Left)]
    public string Name { get; set; } = "";

    [DisplayName("标题")]
    [AutoGenerateColumn(Order = 12, Sortable = true, Filterable = true, Searchable = true, Align = Alignment.Left)]
    public string Title { get; set; } = "";

    [DisplayName("是否隐藏")]
    public bool IsHide { get; set; }

    [DisplayName("内容/值")]
    [SugarColumn(Length = -1)]
    [AutoGenerateColumn(Order = 13, Sortable = true, Filterable = true, Searchable = true, Align = Alignment.Left)]
    public string Value { get; set; } = "";

    [DisplayName("附加")]
    [SugarColumn(Length = -1, IsNullable = true)]
    [AutoGenerateColumn(Order = 13, Sortable = true, Filterable = true, Searchable = true, Align = Alignment.Left)]
    public string? Tag { get; set; }

    [DisplayName("备注")]
    [SugarColumn(Length = -1, IsNullable = true)]
    [AutoGenerateColumn(Order = 14, Sortable = true, Filterable = true, Searchable = true, Align = Alignment.Left)]
    public string? Remark { get; set; }

    [DisplayName("选项值")]
    [SugarColumn(Length = -1, IsNullable = true)]
    [AutoGenerateColumn(Order = 15, Sortable = true, Filterable = true, Searchable = true, Align = Alignment.Left)]
    public string? Items { get; set; }

    [SugarColumn(IsNullable = true)]
    [DisplayName("设置类型")]
    [AutoGenerateColumn(Order = 16, Align = Alignment.Left)]
    public SettingTyp Typ { get; set; }
}
