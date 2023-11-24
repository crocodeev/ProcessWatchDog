using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace CheshkaWatchDog
{

    public class MyJob : IJob
    {
        public Task Execute (IJobExecutionContext context)
        {
        
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string processName = dataMap.GetString("ProcessName");

            Process[] processes = Process.GetProcessesByName(processName);

            foreach (var process in processes)
            {
                process.Kill();
            }

            return Task.CompletedTask;
        }
    }

}
