using AdminSenyun.Core.Attributes;
using AdminSenyun.Server.Services;
using Quartz;
using Quartz.AspNetCore;
using SqlSugar;
using System.ComponentModel;
using System.Reflection;

namespace AdminSenyun.Server.Extensions;

public static class JobExtensions
{
    /// <summary>
    /// 此方法复杂，功能繁琐，实际使用中不需要如此多内容，弃用
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>

    public static IServiceCollection AddJobService(this IServiceCollection services)
    {
        //注册服务
        services.AddQuartz();

        //优雅结束服务
        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });

        //注册 SchedulerService 服务
        services.AddSingleton<IHostedService, SchedulerService>();

        //注册 IScheduler 方便使用时候调用
        services.AddSingleton(provider => provider.GetServices<IHostedService>().OfType<SchedulerService>().First().Scheduler);

        return services;
    }

    public class SchedulerService(ISchedulerFactory schedulerFactory, ISqlSugarClient db, PluginService pluginService) : IHostedService
    {
        public IScheduler Scheduler { get; private set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (schedulerFactory == null) return;

            //启动该后停止数据库中已经指定的服务
            Scheduler = await schedulerFactory.GetScheduler();

            if (Scheduler == null) return;

            //获取数据库中所有任务
            var sysJobs = db.Queryable<SysJob>().ToList();

            var types = pluginService.PluginsTypes.Where(t => typeof(IJob).IsAssignableFrom(t));
            foreach (var item in types)
            {
                var cron = item.GetCustomAttribute<CronAttribute>();
                var description = item.GetCustomAttribute<DescriptionAttribute>();
                var displayName = item.GetCustomAttribute<DisplayNameAttribute>();

                var name = cron?.Name ?? displayName?.DisplayName ?? item.FullName ?? item.Name;

                //查询数据表中的内容
                var sysJob = sysJobs.FirstOrDefault(t => t.Name == name);

                //没有查到数据，插入到数据库中
                if (sysJob is null)
                {
                    sysJob = new SysJob()
                    {
                        Name = name,
                        Cron = cron?.Cron ?? "",
                        IsPaused = false,
                        Description = cron?.Description ?? description?.Description ?? "",
                        Type = item.FullName ?? "",
                    };
                    db.Insertable(sysJob).ExecuteCommand();
                }

                var job = JobBuilder.Create(item).WithIdentity(name).Build();

                var trigger = TriggerBuilder.Create()
                    .WithCronSchedule(sysJob.Cron)
                    .WithDescription(sysJob.Description)
                    .Build();

                await Scheduler.ScheduleJob(job, trigger);

                if (sysJob.IsPaused)
                {
                    await Scheduler.PauseJob(job.Key);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // 任何停止逻辑   
            return Task.CompletedTask;
        }
    }
}