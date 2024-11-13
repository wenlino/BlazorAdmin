using AdminSenyun.Data.Service;
using BootstrapBlazor.Components;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceSysDataExtensions
{
    public static IServiceCollection AddSysDataServices(this IServiceCollection services)
    {
        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

        // 增加业务服务
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IException, ExceptionService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IRole, RoleService>();
        services.AddSingleton<IUser, UserService>();
        services.AddSingleton<ITrace, TraceService>();

        //注入设置服务
        services.AddSingleton<ISysSetting, SysSettingService>();

        //注入文件服务
        services.AddSingleton<SysFileService>();
        return services;
    }
}