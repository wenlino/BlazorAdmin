namespace AdminSenyun.Server.Extensions;

public static class AddBootstrapBlazorExtensions
{

    public static IServiceCollection AddAddBootstrapBlazorService(this IServiceCollection services)
    {
        //注入BootstrapBlazor组件服务
        services.AddBootstrapBlazor();
        //增加 Table Excel 导出服务
        services.AddBootstrapBlazorTableExportService();
        //注入WinBox组件
        services.AddBootstrapBlazorWinBoxService();

        //注入客户端信息        
        return services;
    }
}
