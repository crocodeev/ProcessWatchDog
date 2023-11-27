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

namespace CheshkaWatchDog
{
    public partial class CheskaWatchDog : ServiceBase
    {
        private string target = null;
        private static IScheduler _scheduler;
        EventLog logger = new EventLog();
        System.Timers.Timer timer = new System.Timers.Timer();

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

        public CheskaWatchDog()
        {
            InitializeComponent();
            logger = new EventLog();
            if (!EventLog.SourceExists("CheshkaWatchDog"))
            {
                EventLog.CreateEventSource(
                    "CheshkaWatchDog", "CheshkaWatchDogLogs");
            }
            logger.Source = "CheshkaWatchDog";
            logger.Log = "CheshkaWatchDogLogs";
            logger.Clear();
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        protected override void OnStart(string[] args)
        {
            logger.WriteEntry("Try start");
            //set PENDING status
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            try
            {
                target = GetTarget("RB_*.exe");
            }
            catch (Exception ex)
            {
                logger.WriteEntry(ex.Message);
            }

            //set RUNNING status
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            
            if (target == null)
            {
                logger.WriteEntry("There is no RB_*.exe on this PC");
                SelfStop();
            }
            else
            {

                logger.WriteEntry("Service started");
                
                logger.WriteEntry("Waiting for RS to do job...");
                //Thread.Sleep(180000);
                logger.WriteEntry("Run polling.");

                timer.Elapsed += new ElapsedEventHandler(onTimer);
                timer.Interval = 5000;
                timer.Start();

                logger.WriteEntry("Setting schedule...", EventLogEntryType.Information);
                string schedule = GetSettingFromRegistry();
                logger.WriteEntry(schedule);
                try
                {
                    SetScheduler(schedule);
                }
                catch (Exception ex)
                {

                    logger.WriteEntry(ex.Message, EventLogEntryType.Error);
                }
            }
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown().Wait();
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            logger.WriteEntry("Service stopped at " + DateTime.Now);

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        private void onTimer(object sender, ElapsedEventArgs args)
        {
            Check();
        }

        private void Check()
        {
            string name = GetBaseNameWithoutExtension(target);
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                logger.WriteEntry(this.target + " does not running.");
                ProcessExtension.StartProcessAsCurrentUser(target);
            }
            if (processes.Length > 2)
            {

                foreach (Process process in processes)
                {

                    try
                    {
                        process.Kill();
                    }
                    catch (Exception ex)
                    {

                        logger.WriteEntry($"Error: {ex.Message}");
                    }
                }
            }

        }

        private string Comparator(string a, string b)
        {

            string aBase = Path.GetFileName(a);
            string bBase = Path.GetFileName(b);

            return string.Compare(aBase, bBase, StringComparison.Ordinal) > 0 ? a : b;
        }

        private string GetTarget(string processName)
        {

            string[] programs = Array.Empty<string>();
            string[] disks = { "C", "D", "E" };

            foreach (string disk in disks)
            {
                string directory = $"{disk}:\\Translate";

                if (Directory.Exists(directory)){

                    string[] filenames = Directory.GetFiles(directory, processName, SearchOption.AllDirectories);
                    programs = programs.Concat(filenames).ToArray();
                }
            }

            string target = programs.Aggregate(Comparator);
            return target;
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

                    logger.WriteEntry("Service stopped.");
                }
                else
                {
                    logger.WriteEntry("Service is not running.");
                }
            }
            catch (Exception ex)
            {

                logger.WriteEntry($"Error: {ex.Message}");
            }
        }

        private string GetSettingFromRegistry()
        {
      
            string registryKeyPath = "SOFTWARE\\Inplay\\CheshkaWatchDog\\";

            using (var rootKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (var key = rootKey.OpenSubKey(registryKeyPath, false)) {
                    if (key != null)
                    {
                        return key.GetValue("schedule") as string;
                    }
                }
            }
            return null;
        }

        private string GetBaseNameWithoutExtension(string fullName) {

            string nameWithExtension = Path.GetFileName(fullName);
            string clearName = Path.ChangeExtension(nameWithExtension, null);
            return clearName;
        }

        private void SetScheduler(string schedule)
        {

            if (schedule == null)
            {
                return;
            }

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.Start();

            IEnumerable<TimeObject> timeObjects = TimeConverter.ConvertToTimeObjects(schedule);
            string processName = GetBaseNameWithoutExtension(target);

            foreach (var item in timeObjects)
            {


                IJobDetail job = JobBuilder.Create<MyJob>()
                .WithIdentity($"jb {item.Hour}:{item.Minute}", "group1")
                .UsingJobData("ProcessName", processName) // Pass parameters here
                .Build();

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

                    logger.WriteEntry(ex.Message);
                }

            }

        }
    }
}
