using AdminSenyun.Server.Components;
using Microsoft.AspNetCore.SignalR;
using AdminSenyun.Data.Extensions;
using AdminSenyun.Server.Extensions;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

//注入控制器的服务
builder.Services.AddControllers();

//注入Swagger
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

//注入HttpContext
builder.Services.AddHttpContextAccessor();

//注入Blazor服务
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

//注入身份验证状态
builder.Services.AddCascadingAuthenticationState();

//注入日志
builder.Services.AddLogging();

//注入BootstrapBlazor服务
builder.Services.AddAddBootstrapBlazorService();

//注入权限管理服务
builder.Services.AddServiceAdminService();

//注入LocalStorage 存储服务
builder.Services.AddBlazoredLocalStorage();

//注入数据库链接服务
builder.Services.AddSqlSugarDataServices();

//注入数据服务
builder.Services.AddSysDataServices();

//注入报表服务
builder.Services.AddStimulsoftService();

//注入数据内存缓存服务
builder.Services.AddMemoryCacheServices();

//注入插件中的服务
var plugin = builder.Services.AddPluginService();

//注入Quartz.Net Job模块
builder.Services.AddJobService();



// 增加 SignalR 服务数据传输大小限制配置
builder.Services.Configure<HubOptions>(option => option.MaximumReceiveMessageSize = null);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

//添加Swagger UI
app.UseSwagger().UseSwaggerUI();

app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/404");

app.UseAntiforgery();

//添加客户端信息
app.UseBootstrapBlazor();

//注册Blazor路由服务
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(plugin.AssembliesPages);


app.Run();
