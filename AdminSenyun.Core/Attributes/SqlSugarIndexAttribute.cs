using SqlSugar;

namespace AdminSenyun.Core.Attributes;

public class SqlSugarIndexAttribute : SugarIndexAttribute
{
    public SqlSugarIndexAttribute(string fieldName)
    : base($"Index_{{table}}_{fieldName}", fieldName, OrderByType.Asc, true)
    {
    }
    public SqlSugarIndexAttribute(string? indexName, params string[] fieldNames)
        : base($"Index_{{table}}_{indexName}", fieldNames[0], OrderByType.Asc, true)
    {
        IndexName = "Index_" + (indexName ?? string.Join("_", fieldNames));
        IndexFields.Clear();
        foreach (string item in fieldNames)
        {
            IndexFields.Add(item, OrderByType.Asc);
        }
    }
}
