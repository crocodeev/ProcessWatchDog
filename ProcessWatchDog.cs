using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Runtime.InteropServices;
using Quartz;
using Quartz.Impl;
using Microsoft.Win32;

namespace ProcessWatchDog
{
    public partial class ProcessWatchDog : ServiceBase
    {
        /*
         * Logger 
         */
        private string logSource = "ProcessWatchDog";
        private string logName = "ProcessWatchDogLogs";
        private EventLog logger;
        /*
         * Timer
         */
        private CustomTimer pollingTimer;
        /*
         * Target - реализовать чтение из реестра, пока для теста
         */
        private List<string> targets = new List<string> { };

        private IScheduler _scheduler;
        
        /*
         * Service works
         */
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        public ProcessWatchDog()
        {
            InitializeComponent();
           
            logger = new EventLog();
            if (!EventLog.SourceExists(logSource))
            {
                EventLog.CreateEventSource(
                    logSource, logName);
            }
            logger.Source = logSource;
            
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        /*
         * Service start
         */
        protected override void OnStart(string[] args)
        {
            logger.WriteEntry("Starting...", EventLogEntryType.Information);
            //set PENDING status
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            //set RUNNING status
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            GetTargets(targets);

            if (targets.Count == 0)
            {
                logger.WriteEntry("There is no target processes...", EventLogEntryType.Error);
                SelfStop();
            }

            targets.ForEach(t =>
            {
                if (!Check(t)) {
                    StartProcess(t);
                }
            });

            pollingTimer = new CustomTimer(5000, onPollingTimer);

            string schedule = GetSchedule();



        }

        /*
        * Service stop
        */
        protected override void OnStop()
        {
            try {

                if (!(_scheduler == null)) {
                    _scheduler.Shutdown().Wait();
                }
            }
            catch (Exception ex){
                logger.WriteEntry("scheduler " + ex.Message + ' ' + ex.StackTrace, EventLogEntryType.Error);
            }

            try
            {

                pollingTimer.Stop();
            }
            catch (Exception ex)
            {
                logger.WriteEntry("timer " + ex.Message + ' ' + ex.StackTrace, EventLogEntryType.Error);
            }

            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }


        /*
         * Устаревшее
         */
        private void onPollingTimer(object sender, ElapsedEventArgs args) {

            targets.ForEach(target => {

                if (!Check(target)) {

                    StartProcess(target);
                };
            });
         
        }

        private bool Check(string target)
        {
            string name = GetBaseNameWithoutExtension(target);
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                logger.WriteEntry(target + " does not running...", EventLogEntryType.Warning);
                return false;
            }
            if (processes.Length > 2)
            {
                logger.WriteEntry(target + "runned few times", EventLogEntryType.Warning);
                foreach (Process process in processes)
                {

                    try
                    {
                        process.Kill();
                    }
                    catch (Exception ex)
                    {

                        logger.WriteEntry($"Error: {ex.Message}", EventLogEntryType.Error);
                    }
                }
                return false;
            }

            return true;

        }

        private void StartProcess(string target) 
        {
            try
            {
                ProcessExtension.StartProcessAsCurrentUser(target);
            }
            catch (Exception ex)
            {

                logger.WriteEntry($"Error: {ex.Message}", EventLogEntryType.Error);
            }
            
        }

      
        private void GetTargets(List<string> targets)
        {

            try
            {
                var namesOfProcesses = GetSettingFromRegistry <List<string>>("SOFTWARE\\Inplay\\ProcessWatchDog", "targets");
                targets.AddRange(namesOfProcesses);

            }
            catch (Exception e){ 

                logger.WriteEntry($"Error: {e.Message}", EventLogEntryType.Error);
            }
       
        }

        private void SelfStop()
        {

            string serviceName = this.ServiceName;

            ServiceController serviceController = new ServiceController(serviceName);

            try
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);

                    logger.WriteEntry("Service stopped.", EventLogEntryType.Information);
                }
                else
                {
                    logger.WriteEntry("Service is not running.", EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {

                logger.WriteEntry($"Error: {ex.Message}", EventLogEntryType.Error);
            }
        }

        private T GetSettingFromRegistry<T>(string registryKeyPath, string keyName)
        {

            using (var rootKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (var key = rootKey.OpenSubKey(registryKeyPath, false)) {
                    if (key != null)
                    {

                        object value = key.GetValue(keyName);

                        if (typeof(T) == typeof(List<string>) && value is string[])
                        {

                            return (T)(object)new List<string>((string[])value);
                        }
                        else if (typeof(T) == typeof(string) && value is string)
                        {
                            return (T)value;
                        }
                    }
                }
            }
            return default;
        }

        private string GetBaseNameWithoutExtension(string fullName) {

            string nameWithExtension = Path.GetFileName(fullName);
            string clearName = Path.ChangeExtension(nameWithExtension, null);
            return clearName;
        }

        private string GetSchedule() {

            string schedule = GetSettingFromRegistry <string>("SOFTWARE\\Inplay\\ProcessWatchDog", "schedule");
            return schedule;
        }

        private void SetScheduler(string schedule)
        {

            if (schedule == null)
            {
                logger.WriteEntry("Schedule is empty", EventLogEntryType.Warning);
                return;
            }

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            IEnumerable<TimeObject> timeObjects = TimeConverter.ConvertToTimeObjects(schedule);

            foreach (var item in timeObjects)
            {
                Action<bool,string> callback = new Action<bool, string>(WriteInfo);
                string[] processNames = targets.ToArray();

                IJobDetail job = JobBuilder.Create<RestartJob>()
                .WithIdentity($"jb {item.Hour}:{item.Minute}", "group1")
                .Build();

                job.JobDataMap["processNames"] = processNames;

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"tr {item.Hour}:{item.Minute}", "group1")
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(x => x
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(item.Hour, item.Minute))
                        .WithIntervalInHours(24)
                        .OnEveryDay())
                    .Build();
                try
                {
                    // Schedule the job with the trigger
                    _scheduler.ScheduleJob(job, trigger);

                }
                catch (Exception ex)
                {

                    logger.WriteEntry($"Error: {ex.Message}", EventLogEntryType.Error);
                }

            }

            _scheduler.Start();

        }

        private void WriteInfo(bool isSuccess, string message) {

            if (isSuccess)
            {
                logger.WriteEntry(message, EventLogEntryType.Information);
            }
            else 
            {
                logger.WriteEntry(message, EventLogEntryType.Error);
            }

        }

        /*
         * 
         */
    }
}
