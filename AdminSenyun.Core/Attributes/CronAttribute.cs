namespace AdminSenyun.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CronAttribute(string cron, string? name = null, string? description = null) : Attribute
{
    public string Cron { get; set; } = cron;

    public string? Name { get; set; } = name;

    public string? Description { get; set; } = description;
}
