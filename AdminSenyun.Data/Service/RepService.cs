using BootstrapBlazor.Components;
using Microsoft.Extensions.Options;
using Stimulsoft.Report;
using Stimulsoft.Report.Blazor;
using Stimulsoft.Report.Web;

namespace AdminSenyun.Data.Service;

public class RepService
{
    public readonly StiBlazorViewerOptions Options;
    private readonly ISqlSugarClient db;
    private readonly ISysSetting sysSetting;
    private readonly ICache cache;

    public RepService(ISqlSugarClient db, ISysSetting sysSetting, ICache cache)
    {
        this.db = db;
        this.sysSetting = sysSetting;
        this.cache = cache;
        Options = new StiBlazorViewerOptions();
        Options.Appearance.ScrollbarsMode = true;
        Options.Toolbar.DisplayMode = StiToolbarDisplayMode.Separated;
        Options.Toolbar.ShowAboutButton = false;
    }

    /// <summary>
    /// 获取报表数据
    /// </summary>
    /// <param name="repName"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public StiReport GetStiReport(string repName, string exportName, object parameter)
    {
        var repBase = cache.GetAll<RepBase>()
            .Where(t => t.Name == repName)
            .First() ?? throw new Exception("没有找到报表");

        //获取数据
        var repDatags = cache.GetAll<RepData>()
            .Where(t => t.RepBaseId == repBase.Id)
            .ToList().GroupBy(t => t.DbName);

        var rp = new StiReport();
        rp.LoadFromString(repBase.Report);//加载表表内容
        rp.ReportName = exportName;//设置报表名称
        rp.Compile();//编译报表
        foreach (var repDatas in repDatags)
        {
            var sqlSugarClient = sysSetting.GetSqlSugarClient(repDatas.Key);            
            foreach (var item in repDatas)
            {
                var dt = sqlSugarClient.Ado.GetDataTable(item.Sql, parameter);
                rp.RegData(item.Name, dt);
            }
        }
        rp.Render();
        return rp;
    }

    /// <summary>
    /// 弹出对话框显示报表
    /// </summary>
    /// <param name="stiReport"></param>
    /// <returns></returns>
    public async Task ShowStiReportViewDialog(DialogService dialogService, StiReport stiReport)
    {
        var option = new DialogOption()
        {
            ShowMaximizeButton = true,
            BodyTemplate = builder =>
            {
                //builder.OpenElement(0, "div");
                //builder.AddAttribute(1, "style", "min-height:600px;height:100%");
                //builder.AddContent(2, builder =>
                //{
                builder.OpenComponent<StiBlazorViewer>(0);
                builder.AddComponentParameter(1, nameof(StiBlazorViewer.Report), stiReport);
                builder.AddComponentParameter(2, nameof(StiBlazorViewer.Width), "100%");
                builder.AddComponentParameter(3, nameof(StiBlazorViewer.Height), "100%");
                builder.AddComponentParameter(4, nameof(StiBlazorViewer.Options), Options);
                builder.CloseComponent();
                //});
                //builder.CloseElement();
            },
            ShowFooter = false,
            Size = Size.ExtraLarge,
            Title = stiReport.ReportName + " - 预览",
            FullScreenSize = FullScreenSize.Always,
        };
        await dialogService.Show(option);
    }
}
