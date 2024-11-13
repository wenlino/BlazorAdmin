using AdminSenyun.Data.Service;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceMemoryCacheExtensions
{
    /// <summary>
    /// 注入数据缓存服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMemoryCacheServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICache, MemoryCacheService>();

        return services;
    }
}