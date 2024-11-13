using System.Xml.Linq;

namespace AdminSenyun.Data.Core;

public interface ISysSetting
{
    /// <summary>
    /// 获取设置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    SysSetting this[string key] { get; }
    /// <summary>
    /// 获取所有设置
    /// </summary>
    /// <returns></returns>
    List<SysSetting> GetAll();

    /// <summary>
    /// 获取设置选项
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    SysSetting GetSetting(string key);

    /// <summary>
    /// 获取设置中的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string? GetSettingString(string key);

    /// <summary>
    /// 获取设置中的值转换成int
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    int GetSettingInt(string key);

    /// <summary>
    /// 获取设置中的值转换成long
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    long GetSettingLong(string key);

    /// <summary>
    /// 获取设置中的值转换成decimal
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    decimal GetSettingDecimal(string key);

    /// <summary>
    /// 或者设置中的值BOOL
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool GetSettingBool(string key);

    /// <summary>
    /// 获取设置中的数据库可执行
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ISqlSugarClient? GetSqlSugarClient(string key);

    /// <summary>
    /// 修改或者新增系统设置中的值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool SetSetting(string key, object value);
    /// <summary>
    /// 修改或者新增系统设置中的值
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    bool SetSetting(SysSetting setting);

    /// <summary>
    /// 保存设置数据
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    bool SaveSetting(SysSetting setting, bool isNew);
}
