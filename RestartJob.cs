using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace CheshkaWatchDog
{

    public class RestartJob : IJob
    {

        //private readonly Action<bool, string> callback;
        //private readonly string processName;

        // RestartJob(Action<bool, string> callback, string processName) { 
        //   this.callback = callback;
        //    this.processName = processName;
        //}
        public Task Execute (IJobExecutionContext context)
        {
        
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string processName = dataMap.GetString("processName");

            Process[] processes = Process.GetProcessesByName(processName);

            foreach (var process in processes)
            {
                try 
                { process.Kill();
                  //this.callback.Invoke(true, "Killing " + processName);
                } 
                catch (Exception)
                {
                   //this.callback.Invoke(false, "Exeption killing process: " + ex.Message);  
                }
            }

            return Task.CompletedTask;
        }
    }

}
