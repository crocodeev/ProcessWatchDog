using System;
using System.Collections.Generic;
using System.Linq;
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

        public void DeleteServive() { }

        public void DeleteFolder() { }
    }
}
