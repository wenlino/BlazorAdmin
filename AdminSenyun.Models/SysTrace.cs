using System.ComponentModel.DataAnnotations;

namespace AdminSenyun.Models;

/// <summary>
/// 
/// </summary>
[SugarTable(null, "访问日志")]
public class SysTrace
{
    /// <summary>
    /// 
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "登录用户")]
    public string? UserName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "操作时间")]
    public DateTime LogTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "登录主机")]
    public string? Ip { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "浏览器")]
    public string? Browser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "操作系统")]
    public string? OS { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "操作地点")]
    public string? City { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "操作页面")]
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Referer { get; set; }
}
