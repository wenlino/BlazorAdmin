using AdminSenyun.Core.Attributes;
using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Console = System.Console;

namespace AdminSenyun.Server.Services;

public class PluginService
{
    public PluginService()
    {
        var dlls = new List<string>();

        //发布环境调用目录下的dll文件
        if (Directory.Exists("Plugins"))
            dlls.AddRange(Directory.EnumerateFiles("Plugins", "*.dll", SearchOption.AllDirectories));

        //debug模式下获取项目目录下的文件
#if DEBUG
        var folders = Directory.GetDirectories("..\\Plugins");

        foreach (var folder in folders)
        {
            var name = Path.GetFileName(folder);

            var files = Directory.EnumerateFiles(folder, name + ".dll", SearchOption.AllDirectories);

            if (files.Any())
            {
                var file = files.FirstOrDefault(t => t.Contains("bin", StringComparison.CurrentCultureIgnoreCase) && t.Contains("debug", StringComparison.CurrentCultureIgnoreCase));

                if (file != null)
                {
                    dlls.Add(file);
                }
            }
        }
#endif

        if (dlls.Count > 0)
        {
            foreach (var path in dlls)
            {
                AddDll(path);
            }
        }


        AssembliesPages = PluginsAssemblies.Union([typeof(Sys.Pages._Imports).Assembly]).ToArray();
        AssembliesPagesAll = AssembliesPages.Union([typeof(PluginService).Assembly]);
        PageUrlTypes = GetTypesPages().SelectMany(item => item.GetCustomAttributes<RouteAttribute>()
        .Select(t => new
        {
            Url = string.Join("/", t.Template.Split("/", StringSplitOptions.RemoveEmptyEntries)),
            Type = item
        })).ToDictionary(t => t.Url, t => t.Type, StringComparer.OrdinalIgnoreCase);
    }

    #region 插件信息 类

    public class PluginInfo
    {
        public PluginInfo(string filePath)
        {
            this.FilePath = filePath;

            var assembly = Assembly.LoadFrom(filePath);


            //            //此方法 做到隔离，但是不好是外部dll无法被引用
            //Context = new AssemblyLoadContext(this.Name, true);

            //#if DEBUG
            //            //调试模式下直接加载文件
            //            var fileStream = new FileStream(filePath, FileMode.Open);
            //            var assembly = Context.LoadFromStream(fileStream);
            //#else
            //            //Debug 模式下先存在内存中，然后加载，防止文件锁死
            //            var dllbytes = File.ReadAllBytes(filePath);
            //            var stream = new MemoryStream(dllbytes);

            //            var assembly = Context.LoadFromStream(stream);
            //#endif
            this.Assembly = assembly;
        }

        public string Name => Path.GetFileName(this.FilePath);
        public string FilePath { get; private set; }

        public Assembly Assembly { get; private set; }

        //public AssemblyLoadContext Context { get; private set; }
    }

    public class PluginInfoCollection : List<PluginInfo>
    {
        public PluginInfo Add(string path)
        {
            //移除相同程序集
            var name = Path.GetFileName(path);
            if (this.FirstOrDefault(t => t.Name == name) is PluginInfo info)
            {
                base.Remove(info);
            }

            var pi = new PluginInfo(path);
            base.Add(pi);
            return pi;
        }

        //public void Unload(string path)
        //{
        //    var name = Path.GetFileName(path);

        //    if (this.FirstOrDefault(t => t.Name == name) is PluginInfo info)
        //    {
        //        info.Context.Unload();
        //        base.Remove(info);
        //    }
        //}
    }

    private PluginInfoCollection pluginInfos { get; } = [];

    #endregion

    #region 添加插件


    /// <summary>
    /// 添加dll文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Assembly? AddDll(string path)
    {
        try
        {
            if (!File.Exists(path))
                return null;

            //pluginInfos.Unload(path);

            var pi = pluginInfos.Add(path);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Load dll OK：");
            Console.ResetColor();
            Console.WriteLine(pi.Name);
            return pi.Assembly;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Load dll error：");
            Console.ResetColor();
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    #endregion

    #region 获取或设置插件信息


    /// <summary>
    /// 插件程序集集合
    /// </summary>
    public IEnumerable<Assembly> PluginsAssemblies => pluginInfos.Select(t => t.Assembly);

    /// <summary>
    /// 存在Razor 页面的程序集
    /// </summary>
    public Assembly[] AssembliesPages { get; private set; }

    /// <summary>
    /// 全部程序集
    /// </summary>
    public IEnumerable<Assembly> AssembliesPagesAll { get; private set; }




    /// <summary>
    /// 插件程序集类型
    /// </summary>
    public IEnumerable<Type> PluginsTypes => PluginsAssemblies.SelectMany(t => t.GetTypes());


    /// <summary>
    /// Url地址实例记录
    /// </summary>
    public Dictionary<string, Type> PageUrlTypes { get; private set; }




    /// <summary>
    /// 程序集类型
    /// </summary>
    public IEnumerable<Type> GetTypesPages()
    {
        return AssembliesPagesAll
            .SelectMany(t => t.GetTypes())
            .Where(t => t.GetCustomAttributes<RouteAttribute>().Any());
    }


    /// <summary>
    /// 通过url获取地址对应的实例
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public Type? GetUrlToPageType(string url)
    {
        return PageUrlTypes.TryGetValue(string.Join("/", url.Split("/", StringSplitOptions.RemoveEmptyEntries)), out Type? type) ? type : null;
    }


    #endregion


    #region 热拔插，代码还没做好

    private List<string> WatcherFolders = [];

    private void PluginSystemWatcher(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || WatcherFolders.Contains(path) || !Directory.Exists(path))
            return;

        WatcherFolders.Add(path);


        var watcher = new FileSystemWatcher(path, "*.dll")
        {
            NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
        };

        watcher.Changed += (s, e) =>
        {
            //等待5秒
            //Task.Delay(5000).Wait();
            //AddDll(e.FullPath);

            var name = Path.GetFileName(e.FullPath);

            if (pluginInfos.FirstOrDefault(t => t.Name == name) is PluginInfo info)
            {
                //info.Context.Unload();
                pluginInfos.Remove(info);
            }
        };

        watcher.EnableRaisingEvents = true;
    }

    #endregion



    #region 初始化数据库


    /// <summary>
    /// 插件数据库表初始化
    /// </summary>
    /// <param name="db"></param>
    public void InitcomPluginsDatabase(ISqlSugarClient db)
    {

        foreach (var assembly in PluginsAssemblies)
        {
            var name = assembly.GetName().Name;

            //获数据库存储取插件版本，若版本不一致
            var vis = "0.0";
            try
            {
                vis = db.Queryable<SysVersion>()
                    .Where(t => t.Name == name)
                    .First()?.Version ?? "0.0";
            }
            catch { }
            var filevis = assembly.GetName().Version;

            if (Version.Parse(vis).CompareTo(filevis) < 0)
            {
                var tabletypes = assembly.GetTypes().Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false) && (u.Namespace?.EndsWith("Entity") ?? false)).ToArray();
                //指定当方法 (在当前方法加上这行，这个方法里面会生效)
                db.CurrentConnectionConfig.MoreSettings = new ConnMoreSettings()
                {
                    DisableNvarchar = true,
                    SqlServerCodeFirstNvarchar = true,
                };
                db.CodeFirst.SetStringDefaultLength(254).InitTables(tabletypes);

                //重置系统版本
                db.Deleteable<SysVersion>().Where(t => t.Name == name).ExecuteCommand();
                var version = new SysVersion()
                {
                    Name = name,
                    Version = filevis.ToString(),
                    UpDateTime = DateTime.Now,
                    Description = "插件应用",
                };
                db.Insertable(version).ExecuteCommand();
            }
        }
    }


    /// <summary>
    /// 自动按照程序集 新建表 或者更新表
    /// </summary>
    /// <param name="db">ISqlSugarClient</param>
    /// <param name="stringDefaultLength">数据库字符串默认长度nvarchar(254) 默认 254</param>
    public void InitcomSysDatabase(ISqlSugarClient db, int stringDefaultLength = 254)
    {
        //获取系统版本，若版本不一致
        var vis = "0.0";
        try
        {
            vis = db.Queryable<SysVersion>()
                .Where(t => t.Name == AppDomain.CurrentDomain.FriendlyName)
                .First()?.Version ?? "0.0";
        }
        catch { }
        var filevis = Assembly.GetExecutingAssembly().GetName().Version;
        //若是查询到的版本小于系统设置版本，重置表格
        if (Version.Parse(vis).CompareTo(filevis) < 0)
        {
            //获取全部有效程序集
            var apptypes = typeof(SysSetting).Assembly.GetTypes()
                .Union(typeof(Data.Models.SysUserRole).Assembly.GetTypes());

            var tabletypes = apptypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass &&
            ((u.Namespace?.Contains("AdminSenyun.Models") ?? false) || (u.Namespace?.Contains("AdminSenyun.Data.Models") ?? false)) &&
            (u.IsDefined(typeof(TableAttribute), false) || u.IsDefined(typeof(SugarTable), false))
            ).ToArray();
            //指定当方法 (在当前方法加上这行，这个方法里面会生效)
            db.CurrentConnectionConfig.MoreSettings = new ConnMoreSettings()
            {
                DisableNvarchar = true,
                SqlServerCodeFirstNvarchar = true,
            };
            db.CodeFirst.SetStringDefaultLength(stringDefaultLength).InitTables(tabletypes);

            //重置系统版本
            db.Deleteable<SysVersion>().Where(t => t.Name == AppDomain.CurrentDomain.FriendlyName).ExecuteCommand();
            var version = new SysVersion()
            {
                Name = AppDomain.CurrentDomain.FriendlyName,
                Version = filevis.ToString(),
                UpDateTime = DateTime.Now,
                Description = "系统框架服务",
            };
            db.Insertable(version).ExecuteCommand();
        }
    }

    #endregion


    /// <summary>
    /// 注入子组件中的服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public IServiceCollection AddSubPluginsService(IServiceCollection services)
    {
        var dllServiceType = PluginsTypes.Where(u => u.IsDefined(typeof(AddServiceAttribute)));

        foreach (var type in dllServiceType)
        {
            var methodName = type.GetCustomAttribute<AddServiceAttribute>()?.MethodName;

            if (methodName != null)
            {
                var methodInfo = type.GetMethod(methodName);
                type.GetMethod(methodName)?.Invoke(null, [services]);
            }
        }
        return services;
    }
}
