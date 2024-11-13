namespace AdminSenyun.Models;

public interface IEntityBase
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
    /// <summary>
    /// 创建者名称
    /// </summary>
    public string? CreateUserName { get; set; }
    /// <summary>
    /// 修改者名称
    /// </summary>
    public string? UpdateUserName { get; set; }
}