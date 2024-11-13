using AdminSenyun.Data.Core;
using System.Data;

namespace AdminSenyun.Data.Service;

class DictService(ISqlSugarClient db, ICache cache) : IDict
{
    public bool AuthenticateDemo(string code)
    {
        var ret = false;
        if (!string.IsNullOrEmpty(code))
        {
            var salt = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "授权盐值")?.Code;
            var authCode = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "哈希结果")?.Code;
            if (!string.IsNullOrEmpty(salt))
            {
                ret = LgbCryptography.ComputeHash(code, salt) == authCode;
            }
        }
        return ret;
    }

    public List<SysDict> GetAll() => cache.GetAll<SysDict>();//Dicts ??= db.Queryable<Dict>().ToList();

    private List<SysDict> dicts => GetAll();

    public List<SysDict> GetDicts(string category) => dicts.Where(t => t.Category == category).ToList();

    public int GetCookieExpiresPeriod()
    {
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "Cookie保留时长")?.Code ?? "0";
        _ = int.TryParse(code, out var ret);
        return ret;
    }

    public string GetCurrentLogin()
    {
        return dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "登录界面")?.Code ?? "Login";
    }

    public Dictionary<string, string> GetLogins()
    {
        return dicts.Where(d => d.Category == "系统首页").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
    }

    public string? GetNotificationUrl(string appId) => GetUrlByName(appId, "系统通知地址");

    public string? GetProfileUrl(string appId) => GetUrlByName(appId, "个人中心地址");

    public string? GetSettingsUrl(string appId) => GetUrlByName(appId, "系统设置地址");

    public Dictionary<string, string> GetThemes()
    {
        return dicts.Where(d => d.Category == "网站样式").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
    }

    public string GetWebFooter()
    {
        var dict = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站页脚");
        return dict?.Code ?? "网站页脚";
    }

    public string GetWebTitle()
    {
        var dict = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站标题");
        return dict?.Code ?? "网站标题";
    }

    public string GetWebLoginFooter()
    {
        var dict = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "登录底部");
        return dict?.Code ?? "森云科技出品";
    }

    public string GetWebSubTitle()
    {
        var dict = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站副标题");
        return dict?.Code ?? "网站标题";
    }


    private bool SaveDict(SysDict dict)
    {
        //检查是否存在，不存在新增
        var result = db.Queryable<SysDict>().Where(t => t.Category == dict.Category && t.Name == dict.Name).Any();
        var ret = false;
        if (result)
        {
            ret = db.Updateable<SysDict>().Where(s => s.Category == dict.Category && s.Name == dict.Name).SetColumns(s => s.Code, dict.Code).ExecuteCommand() > 0;
        }
        else
        {
            ret = db.Insertable(dict).ExecuteCommand() > 0;
        }
        if (ret)
        {
            // 更新缓存
            cache.Remove<SysDict>();
        }
        return ret;
    }

    public bool SaveCookieExpiresPeriod(int expiresPeriod) => SaveDict(new SysDict { Category = "网站设置", Name = "Cookie保留时长", Code = expiresPeriod.ToString() });

    public bool SaveHealthCheck(bool enable = true) => SaveDict(new SysDict { Category = "网站设置", Name = "演示系统", Code = enable ? "1" : "0" });

    public bool SaveLogin(string login) => SaveDict(new SysDict { Category = "网站设置", Name = "登录界面", Code = login });

    public bool SaveTheme(string theme) => SaveDict(new SysDict { Category = "网站设置", Name = "使用样式", Code = theme });

    public bool SaveWebFooter(string footer) => SaveDict(new SysDict { Category = "网站设置", Name = "网站页脚", Code = footer });

    public bool SaveWebTitle(string title) => SaveDict(new SysDict { Category = "网站设置", Name = "网站标题", Code = title });
    public bool SaveWebSubTitle(string title) => SaveDict(new SysDict { Category = "网站设置", Name = "网站副标题", Code = title });

    private string? GetUrlByName(string appId, string dictName)
    {
        string? url = null;
        var appName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId)?.Name;
        if (!string.IsNullOrEmpty(appName))
        {
            url = dicts.FirstOrDefault(d => d.Category == appName && d.Name == dictName)?.Code;
        }
        return url;
    }

    public bool GetEnableDefaultApp()
    {
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "默认应用程序")?.Code ?? "0";
        return code == "1";
    }

    public string GetIconFolderPath()
    {
        return dicts.FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址")?.Code ?? "/images/uploder/";
    }

    public string GetDefaultIcon()
    {
        return dicts.FirstOrDefault(d => d.Name == "头像文件" && d.Category == "头像地址")?.Code ?? "default.png";
    }

    public string? GetIpLocatorName()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "IP地理位置接口")?.Code;
    }

    public string? GetIpLocatorUrl(string? name)
    {
        return string.IsNullOrWhiteSpace(name) ? null : dicts.FirstOrDefault(s => s.Category == "地理位置" && s.Name == name)?.Code;
    }

    public bool GetAppSiderbar()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "侧边栏状态")?.Code == "1";
    }

    public bool SaveAppSiderbar(bool value) => SaveDict(new SysDict { Category = "网站设置", Name = "侧边栏状态", Code = value ? "1" : "0" });

    public bool GetAppTitle()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "卡片标题状态")?.Code == "1";
    }

    public bool SaveAppTitle(bool value) => SaveDict(new SysDict { Category = "网站设置", Name = "卡片标题状态", Code = value ? "1" : "0" });

    public bool GetAppFixHeader()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "固定表头")?.Code == "1";
    }

    public bool SaveAppFixHeader(bool value) => SaveDict(new SysDict { Category = "网站设置", Name = "固定表头", Code = value ? "1" : "0" });

    public bool GetAppHealthCheck()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "健康检查")?.Code == "1";
    }

    public bool SaveAppHealthCheck(bool value) => SaveDict(new SysDict { Category = "网站设置", Name = "健康检查", Code = value ? "1" : "0" });

    public bool GetAppMobileLogin()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "短信验证码登录")?.Code == "1";
    }

    public bool SaveAppMobileLogin(bool value) => SaveDict(new SysDict { Category = "网站设置", Name = "短信验证码登录", Code = value ? "1" : "0" });

    public bool GetAppOAuthLogin()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "OAuth 认证登录")?.Code == "1";
    }

    public bool SaveAppOAuthLogin(bool value) => SaveDict(new SysDict { Category = "网站设置", Name = "OAuth 认证登录", Code = value ? "1" : "0" });

    public bool GetAutoLockScreen()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "自动锁屏")?.Code == "1";
    }

    public bool SaveAutoLockScreen(bool value) => SaveDict(new SysDict { Category = "网站设置", Name = "自动锁屏", Code = value ? "1" : "0" });

    public int GetAutoLockScreenInterval()
    {
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "自动锁屏时长")?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveAutoLockScreenInterval(int value) => SaveDict(new SysDict { Category = "网站设置", Name = "自动锁屏时长", Code = value.ToString() });

    public Dictionary<string, string> GetIpLocators()
    {
        return dicts.Where(d => d.Category == "地理位置服务").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
    }

    public string? GetIpLocator()
    {
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "IP地理位置接口")?.Code;
    }

    public bool SaveCurrentIp(string value) => SaveDict(new SysDict { Category = "网站设置", Name = "IP地理位置接口", Code = value });

    public int GetCookieExpired()
    {
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "Cookie保留时长")?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveCookieExpired(int value) => SaveDict(new SysDict { Category = "网站设置", Name = "Cookie保留时长", Code = value.ToString() });

    public int GetExceptionExpired()
    {
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "程序异常保留时长")?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveExceptionExpired(int value) => SaveDict(new SysDict { Category = "网站设置", Name = "程序异常保留时长", Code = value.ToString() });

    public int GetOperateExpired()
    {
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "操作日志保留时长")?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveOperateExpired(int value) => SaveDict(new SysDict { Category = "网站设置", Name = "操作日志保留时长", Code = value.ToString() });

    public int GetLoginExpired()
    {
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "登录日志保留时长")?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveLoginExpired(int value) => SaveDict(new SysDict { Category = "网站设置", Name = "登录日志保留时长", Code = value.ToString() });

    public int GetAccessExpired()
    {
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "访问日志保留时长")?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveAccessExpired(int value) => SaveDict(new SysDict { Category = "网站设置", Name = "访问日志保留时长", Code = value.ToString() });

    public int GetIPCacheExpired()
    {
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "IP请求缓存时长")?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveIPCacheExpired(int value) => SaveDict(new SysDict { Category = "网站设置", Name = "IP请求缓存时长", Code = value.ToString() });
}
