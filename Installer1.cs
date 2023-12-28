using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheshkaWatchDog
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            string schedule = Context.Parameters["schedule"];

            InstallerHelper.WriteToRegisry(schedule);

        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);

            
            try
            {
                InstallerHelper.DeleteService("chesckaWatchDog");
            }
            catch (Exception ex)
            {

                MessageBox.Show(
                        $"{ex}",
                        "Error occured during uninstall chesckaWatchDog, please, do it manually",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }

            try
            {
                InstallerHelper.DeleteService("cheshkaWatchDog2");
            }
            catch (Exception ex)
            {

                MessageBox.Show(
                        $"{ex}",
                        "Error occured during uninstall chesckaWatchDog2, please, do it manually",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }

            try
            {
                InstallerHelper.DeleteExe("C:\\cheshkaWatchDog.exe");
            }
            catch (Exception ex)
            {

                MessageBox.Show(
                        $"{ex}",
                        "Error occured during delete cheshkaWatchDog.exe, please, do it manually",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }

            try
            {
                InstallerHelper.DeleteExe("C:\\cheshkaWatchDog2.exe");
            }
            catch (Exception ex)
            {

                MessageBox.Show(
                        $"{ex}",
                        "Error occured during delete cheshkaWatchDog2.exe, please, do it manually",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }

            try
            {
                InstallerHelper.DeleteFolder("C:\\cheshkaWatchDog");
            }
            catch (Exception ex)
            {

                MessageBox.Show(
                        $"{ex}",
                        "Error occured during remove C:\\cheshkaWatchDog folder, please, do it manually",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }

            try
            {
                InstallerHelper.DeleteFolder("C:\\cheshkaWatchDogLogs");
            }
            catch (Exception ex)
            {

                MessageBox.Show(
                        $"{ex}",
                        "Error occured during remove C:\\cheshkaWatchDogLogs folder, please, do it manually",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }
            
        }

    }
}
