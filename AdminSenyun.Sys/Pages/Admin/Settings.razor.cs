﻿using AdminSenyun.Sys.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace AdminSenyun.Sys.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Settings
{
    private bool IsDemo { get; set; }

    [NotNull]
    private AppInfo? AppInfo { get; set; }

    [NotNull]
    private List<SelectedItem>? Logins { get; set; }

    [NotNull]
    private List<SelectedItem>? Themes { get; set; }

    [NotNull]
    private List<SelectedItem>? IPLocators { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [CascadingParameter]
    [NotNull]
    private Layout? Layout { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Logins = DictService.GetLogins().ToSelectedItemList().OrderBy(i => i.Value).ToList();
        Themes = DictService.GetThemes().ToSelectedItemList();
        IPLocators = DictService.GetIpLocators().ToSelectedItemList();
        IPLocators.Insert(0, new SelectedItem("", "未选择"));

        AppInfo = new()
        {
            Title = DictService.GetWebTitle(),
            SubTitle = DictService.GetWebSubTitle(),
            Footer = DictService.GetWebFooter(),
            Login = DictService.GetCurrentLogin(),
            SiderbarSetting = DictService.GetAppSiderbar(),
            TitleSetting = DictService.GetAppTitle(),
            FixHeaderSetting = DictService.GetAppFixHeader(),
            HealthCheckSetting = DictService.GetAppHealthCheck(),
            EnableDefaultApp = DictService.GetEnableDefaultApp(),
            MobileLogin = DictService.GetAppMobileLogin(),
            OAuthLogin = DictService.GetAppOAuthLogin(),
            AutoLock = DictService.GetAutoLockScreen(),
            Interval = DictService.GetAutoLockScreenInterval(),
            ExceptionExpired = DictService.GetExceptionExpired(),
            OperateExpired = DictService.GetOperateExpired(),
            LoginExpired = DictService.GetLoginExpired(),
            AccessExpired = DictService.GetAccessExpired(),
            CookieExpired = DictService.GetCookieExpiresPeriod(),
            IPCacheExpired = DictService.GetIPCacheExpired(),
            IpLocator = DictService.GetIpLocator()
        };
    }

    private async Task ShowToast(bool result, string title)
    {
        if (result)
        {
            await ToastService.Success(title, $"保存{title}成功");
        }
        else
        {
            await ToastService.Error(title, $"保存{title}失败");
        }
    }

    private async Task OnSaveTitle(EditContext context)
    {
        var ret = DictService.SaveWebTitle(AppInfo.Title);
        await RenderLayout("title");
        await ShowToast(ret, "网站标题");
    }
    private async Task OnSaveSubTitle(EditContext context)
    {
        var ret = DictService.SaveWebSubTitle(AppInfo.SubTitle);
        await RenderLayout("subtitle");
        await ShowToast(ret, "网站副标题");
    }

    private async Task OnSaveFooter(EditContext context)
    {
        var ret = DictService.SaveWebFooter(AppInfo.Footer);
        await RenderLayout("footer");
        await ShowToast(ret, "网站页脚");
    }

    private async Task OnSaveLogin(EditContext context)
    {
        var ret = DictService.SaveLogin(AppInfo.Login);
        await ShowToast(ret, "登录界面");
    }

    private async Task OnSaveTheme(EditContext context)
    {
        var ret = DictService.SaveLogin(AppInfo.Login);
        await ShowToast(ret, "网站主题");
    }

    private async Task OnSaveAppFeatures(EditContext context)
    {
        var ret = DictService.SaveAppSiderbar(AppInfo.SiderbarSetting);
        DictService.SaveAppTitle(AppInfo.TitleSetting);
        DictService.SaveAppFixHeader(AppInfo.FixHeaderSetting);
        DictService.SaveAppHealthCheck(AppInfo.HealthCheckSetting);
        await ShowToast(ret, "网站功能");
    }

    private async Task OnSaveSaveAppLogin(EditContext context)
    {
        var ret = DictService.SaveAppMobileLogin(AppInfo.MobileLogin);
        DictService.SaveAppOAuthLogin(AppInfo.OAuthLogin);
        await ShowToast(ret, "网站登录");
    }

    private async Task OnSaveAppLockScreen(EditContext context)
    {
        var ret = DictService.SaveAutoLockScreen(AppInfo.AutoLock);
        DictService.SaveAutoLockScreenInterval(AppInfo.Interval);
        await ShowToast(ret, "自动锁屏");
    }

    private async Task SaveAdressInfo(EditContext context)
    {
        var ret = DictService.SaveCurrentIp(AppInfo.IpLocator!);
        await ShowToast(ret, "地理位置");
    }

    private async Task OnSaveLogCache(EditContext context)
    {
        var ret = DictService.SaveCookieExpiresPeriod(AppInfo.CookieExpired);
        DictService.SaveExceptionExpired(AppInfo.ExceptionExpired);
        DictService.SaveAccessExpired(AppInfo.AccessExpired);
        DictService.SaveOperateExpired(AppInfo.OperateExpired);
        DictService.SaveLoginExpired(AppInfo.LoginExpired);
        DictService.SaveIPCacheExpired(AppInfo.IPCacheExpired);

        await ShowToast(ret, "日志缓存");
    }

    private async Task RenderLayout(string key)
    {
        if (Layout.OnUpdateAsync != null)
        {
            await Layout.OnUpdateAsync(key);
        }
    }
}
