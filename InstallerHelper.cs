using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CheshkaWatchDog
{
    public class InstallerHelper
    {
        public static void WriteToRegisry(string schedule)
        {

            string watchDogPath = "SOFTWARE\\Inplay\\CheshkaWatchDog\\";

            try
            {
                RegistryKey root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

                if (root.OpenSubKey(watchDogPath) == null)
                {
                    RegistryKey result = root.CreateSubKey(watchDogPath);

                }


                RegistryKey key = root.OpenSubKey(watchDogPath, true);

                if (key == null) { throw new Exception("Can't create new key in registry"); }

                key.SetValue("schedule", schedule);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace + ' ' + ex.Message);
            }

        }

        public static void StopServices(string[] servicesNames)
        {

            ServiceController[] services = ServiceController.GetServices();

            foreach (string name in servicesNames)
            {
                try
                {
                    ServiceController service = services.First(s => s.ServiceName == name);
                    service.Stop();
                }
                catch (Exception)
                {


                }

            }
        }

        static bool ServiceExists(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                if (service.ServiceName == serviceName)
                {
                    return true;
                }
            }

            return false;
        }

        static void StopAndRemoveService(string serviceName)
        {
            using (ServiceController serviceController = new ServiceController(serviceName))
            {
                if (serviceController.Status != ServiceControllerStatus.Stopped)
                {
                    // Stop the service if it is not already stopped
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                }

                // Use ServiceInstaller to uninstall the service
                using (ServiceInstaller installer = new ServiceInstaller())
                {
                    installer.Context = new InstallContext();
                    installer.ServiceName = serviceName;
                    installer.Uninstall(null);
                }
            }
        }

        public static void DeleteService(string serviceName)
        {

            if (ServiceExists(serviceName))
            {
                StopAndRemoveService(serviceName);
            }
        }

        public static void DeleteFolder(string path)
        {

            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace + ' ' + ex.Message);
            }

        }

        public static void DeleteExe(string path)
        {

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace + ' ' + ex.Message);
            }
        }
    }
}
