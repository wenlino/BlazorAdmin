using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

/// <summary>
/// 登录用户信息实体类
/// </summary>
[SugarTable(null, "登录日志")]
public class SysLoginLog
{
    /// <summary>
    /// 获得/设置 Id
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 获得/设置 用户名
    /// </summary>
    [DisplayName("登录名称")]
    [NotNull]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 登录时间
    /// </summary>
    [DisplayName("登录时间")]
    public DateTime LoginTime { get; set; }

    /// <summary>
    /// 获得/设置 登录IP地址
    /// </summary>
    [DisplayName("主机")]
    [NotNull]
    public string? Ip { get; set; }

    /// <summary>
    /// 获得/设置 登录浏览器
    /// </summary>
    [DisplayName("浏览器")]
    [NotNull]
    public string? Browser { get; set; }

    /// <summary>
    /// 获得/设置 登录操作系统
    /// </summary>
    [DisplayName("操作系统")]
    [NotNull]
    public string? OS { get; set; }

    /// <summary>
    /// 获得/设置 登录地点
    /// </summary>
    [DisplayName("登录地点")]
    [NotNull]
    public string? City { get; set; }

    /// <summary>
    /// 获得/设置 登录是否成功
    /// </summary>
    [DisplayName("登录结果")]
    [NotNull]
    public string? Result { get; set; }

    /// <summary>
    /// 获得/设置 用户 UserAgent
    /// </summary>
    [DisplayName("登录名称")]
    [NotNull]
    public string? UserAgent { get; set; }
}
