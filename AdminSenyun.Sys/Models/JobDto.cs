using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Quartz;
using System.ComponentModel;

namespace AdminSenyun.Sys.Models;

class JobDto
{
    public JobDto()
    {
    }

    public static async Task<JobDto?> CreateAsync(IScheduler scheduler, ITrigger trigger)
    {
        if (scheduler != null && trigger != null && trigger.JobKey != null)
        {
            var jobDetail = await scheduler.GetJobDetail(trigger.JobKey);

            var cronExpressionString = (trigger is Quartz.Impl.Triggers.CronTriggerImpl ct) ? ct.CronExpressionString : "";
            var jobdto = new JobDto()
            {
                Scheduler = scheduler,
                Trigger = trigger,
                CronExpressionString = cronExpressionString,
                Description = trigger?.Description,
                JobDetail = jobDetail,
            };

            return jobdto;
        }
        return null;
    }

    public IJobDetail? JobDetail { get; private set; }
    public ITrigger Trigger { get; private set; }
    public IScheduler Scheduler { get; private set; }


    public JobKey JobKey => this.Trigger.JobKey;
    public TriggerKey TriggerKey => this.Trigger.Key;

    [Description("名称")]
    public string? Name => JobDetail?.Key.Name;

    [Description("注释")]
    public string? Description { get; set; }

    [Description("Cron表达式")]
    public string? CronExpressionString { get; set; }

    [Description("下一次运行时间")]
    public DateTime? NextFireTime => Trigger.GetNextFireTimeUtc()?.LocalDateTime;

    [Description("开始时间")]
    public DateTime? StartTime => Trigger.StartTimeUtc.LocalDateTime;

    [Description("结束时间")]
    public DateTime? EndTime => Trigger.EndTimeUtc?.LocalDateTime;

    [Description("Job类型")]
    public Type JobType => JobDetail.JobType;

    private TriggerState? _state;

    [Description("状态")]
    public TriggerState State => _state ??= Scheduler.GetTriggerState(TriggerKey).GetAwaiter().GetResult();

    [Description("暂停")]
    public bool IsPaused => State == TriggerState.Paused;
 

    /// <summary>
    /// 执行暂停
    /// </summary>
    public void PausedJob()
    {
        Scheduler.PauseJob(JobKey);
        _state = null;
    }

    /// <summary>
    /// 执行开始
    /// </summary>
    public void ResumeJob()
    {
        Scheduler.ResumeJob(JobKey);
        _state = null;
    }

    /// <summary>
    /// 更新Cron表达式 刷新任务
    /// </summary>
    /// <returns></returns>
    public async Task UploadJobServerAsync()
    {
        //var c = (this.Trigger is Quartz.Impl.Triggers.CronTriggerImpl ct) ? ct.CronExpressionString : "";
        var newTrigger = Trigger.GetTriggerBuilder()
        .WithCronSchedule(CronExpressionString)
        .WithDescription(Description)
        .Build();
        await Scheduler.RescheduleJob(TriggerKey, newTrigger);
    }
}
