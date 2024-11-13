using AdminSenyun.Data.Core;

namespace AdminSenyun.Data.Service;

class LoginService(ISqlSugarClient db) : ILogin
{
    public bool Log(string userName, string? IP, string? OS, string? browser, string? address, string? userAgent, bool result)
    {
        var loginUser = new SysLoginLog()
        {
            Id = SnowFlakeSingle.Instance.NextId(),
            UserName = userName,
            LoginTime = DateTime.Now,
            Ip = IP,
            City = address,
            OS = OS,
            Browser = browser,
            UserAgent = userAgent,
            Result = result ? "登录成功" : "登录失败"
        };
        db.Insertable(loginUser).ExecuteCommand();
        return true;
    }
}
