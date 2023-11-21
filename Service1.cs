using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;

namespace CheshkaWatchDog
{
    public partial class Service1 : ServiceBase
    {
        EventLog logger = new EventLog();
        Timer timer = new Timer();
        String processName = "notepad";

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

        public Service1()
        {
            InitializeComponent();
            logger = new EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("CheshkaWatchDog"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "CheshkaWatchDog", "CheshkaWatchDogLogs");
            }
            logger.Source = "CheshkaWatchDog";
            logger.Log = "CheshkaWatchDogLogs";
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        protected override void OnStart(string[] args)
        {
            //set PENDING status
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            logger.WriteEntry("Service started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(onTimer);
            timer.Interval = 5000;
            timer.Start();

            //set RUNNING status
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            logger.WriteEntry("Service stopped at " + DateTime.Now);

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        private void onTimer(object sender, ElapsedEventArgs args) {
            Check();
        }

        private void Check()
        {
            Process[] processes = Process.GetProcessesByName(this.processName);
            if (processes.Length == 0) {
                logger.WriteEntry(this.processName + " does not exist");
                ProcessExtension.StartProcessAsCurrentUser("notepad.exe");
            }
        }
    }
}
