using AdminSenyun.Data.Core;
using AdminSenyun.Data.Extensions;
using AdminSenyun.Data.Service;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Xml.Linq;
using Console = System.Console;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// SqlSugar ORM 注入服务扩展类
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注入 SqlSugar 数据服务类
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddSqlSugarDataServices(this IServiceCollection services, Action<ISqlSugarClient>? dbAction = null)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        var connString = configuration.GetConnectionString("DefaultConnection");

        var dbType = Enum.TryParse(configuration.GetConnectionString("DbType"), out DbType result) ? result : DbType.SqlServer;

        var config = new ConnectionConfig()
        {
            ConnectionString = connString,
            DbType = dbType,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
            ConfigureExternalServices = SqlSugarHelper.InitConfigureExternalServices()
        };

        var sqlSugar = new SqlSugarScope(config, db =>
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                //生成自定义参数 便于复制内容调试

                var sqlString = "\r\n" + string.Join("\r\n", pars.Select(t => $"DECLARE {t.ParameterName} NVARCHAR(MAX) = N'{t.Value}'")) + "\r\n" + sql + "\r\n";

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SqlServer Run Code:");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(sqlString);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Sql Native:");
                Console.ResetColor();
                //获取原生SQL推荐 5.1.4.63  性能OK
                Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));

                //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
                //var a = UtilMethods.GetSqlString(DbType.SqlServer, sql, pars);

                //Debug.WriteLine(UtilMethods.GetSqlString(DbType.SqlServer, sql, pars));

            };

            db.Aop.OnError = e =>
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Sql);
            };



            db.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    // 主键(long类型)且没有值的---赋值雪花Id
                    if (entityInfo.EntityColumnInfo.IsPrimarykey)
                    {
                        var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);

                        if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long) && (id == null || (long)id == 0))
                        {
                            entityInfo.SetValue(SnowFlakeSingle.Instance.NextId());
                        }
                        else if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(string) && (string.IsNullOrWhiteSpace(id?.ToString()) || id?.ToString() == "0"))
                        {
                            entityInfo.SetValue(SnowFlakeSingle.Instance.NextId().ToString());
                        }
                    }

                    if (entityInfo.PropertyName == "CreateTime")
                        entityInfo.SetValue(DateTime.Now);

                    if (entityInfo.PropertyName == "CreateUserName")
                    {
                        var identity = services.BuildServiceProvider().GetService<IdentityService>();
                        entityInfo.SetValue(identity?.UserName ?? "");
                    }
                }

                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.PropertyName == "UpdateTime")
                        entityInfo.SetValue(DateTime.Now);
                    if (entityInfo.PropertyName == "UpdateUserName")
                    {
                        var identity = services.BuildServiceProvider().GetService<IdentityService>();
                        entityInfo.SetValue(identity?.UserName ?? "");
                    }
                }
            };


        });
        services.AddSingleton<ISqlSugarClient>(sqlSugar);

        dbAction?.Invoke(sqlSugar);

        return services;
    }
}