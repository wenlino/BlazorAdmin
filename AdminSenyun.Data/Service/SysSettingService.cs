using AdminSenyun.Data.Extensions;
using AdminSenyun.Models;

namespace AdminSenyun.Data.Service;

public class SysSettingService(ISqlSugarClient db, ICache cache) : ISysSetting
{
    public List<SysSetting> Settings => cache.GetAll<SysSetting>();

    public SysSetting? this[string key] => GetSetting(key);

    public List<SysSetting> GetAll()
    {
        return Settings;
    }

    public SysSetting? GetSetting(string key)
    {
        var val = Settings.Where(t => t?.Name?.ToLower() == key?.ToLower());
        if (val.Any())
            return val.First();
        else
            return null;
    }
    public string? GetSettingString(string key)
    {
        return this[key]?.Value;
    }
    public bool GetSettingBool(string key)
    {
        return this[key]?.Value == "是";
    }

    public decimal GetSettingDecimal(string key)
    {
        var val = this[key]?.Value;
        return Convert.ToDecimal(val);
    }

    public int GetSettingInt(string key)
    {
        var val = this[key]?.Value;
        return Convert.ToInt32(val);
    }

    public long GetSettingLong(string key)
    {
        var val = this[key]?.Value;
        return Convert.ToInt64(val);
    }



    public ISqlSugarClient? GetSqlSugarClient(string key)
    {
        var se = this[key];
        if (se is null) return null;

        var dbtype = DbType.Sqlite;

        if (se.Typ == SettingTyp.SqlServer)
            dbtype = DbType.SqlServer;
        else if (se.Typ == SettingTyp.Access)
            dbtype = DbType.Access;
        else if (se.Typ == SettingTyp.Sqlite)
            dbtype = DbType.Sqlite;

        var connection = new ConnectionConfig()
        {
            ConnectionString = se.Value,
            MoreSettings = new ConnMoreSettings() { IsNoReadXmlDescription = true },
            DbType = dbtype,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
            ConfigureExternalServices = SqlSugarHelper.InitConfigureExternalServices()
        };

        return new SqlSugarScope(connection, db =>
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
            };
            db.Aop.OnError = (s) =>
            {
                Console.WriteLine(s.Sql);
            };
        });
    }

    public bool SetSetting(string key, object value)
    {
        try
        {
            string val = (value is bool result) ? (result ? "是" : "否") : (value?.ToString() ?? "");
            var config = GetSetting(key);
            if (config is null)
            {
                config = new SysSetting()
                {
                    Name = key,
                    Title = key,
                    Typ = SettingTyp.String,
                    Value = val,
                };
                if (value is bool)
                {
                    config.Typ = SettingTyp.Bool;
                }
                db.Insertable(config).ExecuteCommand();
            }
            else
            {
                config.Value = val;
                db.Updateable(config).ExecuteCommand();
            }
            cache.Remove<ISysSetting>();
            return true;
        }
        catch { return false; }
    }

    public bool SetSetting(SysSetting setting)
    {
        try
        {
            //查看名称是否存在
            var sysv = GetSetting(setting.Name);

            if (sysv is null)
            {
                db.Insertable(setting).ExecuteCommand();
            }
            else
            {
                sysv.Value = setting.Value;
                db.Updateable(sysv).ExecuteCommand();
            }
            cache.Remove<ISysSetting>();
            return true;
        }
        catch { return false; }

    }

    public bool SaveSetting(SysSetting setting, bool isNew)
    {
        var result = db.Queryable<SysSetting>()
            .Where(t => t.Id != setting.Id)
            .Where(t => t.Name == setting.Name)
            .Any();
        if (result)
        {
            return false;
        }
        if (isNew)
            db.Insertable(setting).ExecuteCommand();
        else
            db.Updateable(setting).ExecuteCommand();
        //清理缓存
        cache.Remove<ISysSetting>();
        return true;
    }
}