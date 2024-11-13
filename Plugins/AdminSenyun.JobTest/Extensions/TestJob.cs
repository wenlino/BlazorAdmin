using AdminSenyun.Core.Attributes;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSenyun.JobTest.Extensions
{
    [Description("测试服务定时服务")]
    [Cron("* * * */1 * ?")]
    [DisplayName("测试测试")]
    [DisallowConcurrentExecution]
    internal class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(context.NextFireTimeUtc?.LocalDateTime);
            Console.WriteLine(context.JobRunTime);
            Console.WriteLine(context.Trigger);
            Console.WriteLine(context.JobDetail);
            Task.Delay(1000).Wait();
            Console.WriteLine("执行任务完成");
            return Task.CompletedTask;
        }
    }
}
