using AdminSenyun.Data.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdminSenyun.Server.Extensions;

public static class StimulsoftExtensions
{
    public static IServiceCollection AddStimulsoftService(this IServiceCollection services)
    {
        //此key用于测试，内容无效，无法正常使用，若需要用sti报表，请自行联系供应方购买正版Key

        Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHnJgkVxrq2w/eP1fRqkw8G6TfpASWq9MOt//wchj6ZzQ0Di2ZG9dq4s4cid/QE9kW2cApgoGlrOM33Rbw1S3hCRjebz71ZNJxvclhVjAlGKM7AKkGUkDGLNlNa3irZjvpkFSy3Zhdz4XcJBf2ASyYr2qPjjo6gGENNefzgDEksqt7TWILZCb5qIPC5KW+qCV2mJsRSX5Esk2QPEWkQRicsGi7WX4NQ4ZVA/aKRAvc40EkUc6MxPp+BOr07vGEM5u0xBK4nPksj26NLTK7K9on8y";

        Stimulsoft.Base.Localization.StiLocalization.Localization = "zh-CHS.xml";

        //添加中文字体
        Stimulsoft.Base.StiFontCollection.AddFontFile("wwwroot/font/AlibabaPuHuiTi-3-35-Thin.ttf");

        services.TryAddSingleton<RepService>();

        return services;
    }
}