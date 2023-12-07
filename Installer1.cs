using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

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

            InstallerHelper.DeleteService("chesckaWatchDog");
            InstallerHelper.DeleteService("cheshkaWatchDog2");
            InstallerHelper.DeleteExe("C:\\cheshkaWatchDog.exe");
            InstallerHelper.DeleteExe("C:\\cheshkaWatchDog2.exe");
            InstallerHelper.DeleteFolder("C:\\cheshkaWatchDog");
            InstallerHelper.DeleteFolder("C:\\cheshkaWatchDogLogs");
        }

    }
}
