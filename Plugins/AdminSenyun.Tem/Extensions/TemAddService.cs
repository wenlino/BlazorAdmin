using AdminSenyun.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AdminSenyun.Tem.Extensions;

[AddService(nameof(AddService))]
public static class TemAddService
{
    public static void AddService(this IServiceCollection services)
    {
        //插件中若需要注入服务  在类上标记  [AddService(XXX)] XXX 方法名称
    }
}