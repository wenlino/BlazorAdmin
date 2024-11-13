namespace AdminSenyun.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DictAttribute(string category) : Attribute
{
    public string Category { get; set; } = category;
}
