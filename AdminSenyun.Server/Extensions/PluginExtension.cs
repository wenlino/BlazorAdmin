using AdminSenyun.Server.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;

namespace AdminSenyun.Server.Extensions;

public static class PluginExtension
{
    public static PluginService AddPluginService(this IServiceCollection services)
    {
        var plugin = new PluginService();

        //初始化数据库
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        if (configuration.GetValue<bool>("SysSettings:InitDatabaseTable"))
        {
            var db = services.BuildServiceProvider().GetRequiredService<ISqlSugarClient>();
            plugin.InitcomSysDatabase(db);
            plugin.InitcomPluginsDatabase(db);
        }

        //注入插件中书写的服务
        plugin.AddSubPluginsService(services);

        //注入服务，提供全局调用
        services.TryAddSingleton(plugin);

        //返回插件实例，方便首次加载调用
        return plugin;
    }
}
