using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Runtime.CompilerServices;

namespace CheshkaWatchDogConfigurator
{
    public partial class Form1 : Form
    {

        private ServiceController serviceController;
        public Form1()
        {
            InitializeComponent();
        }

        private string ReadFromRegistry() {

            string watchDogPath = "SOFTWARE\\Inplay\\CheshkaWatchDog\\";
            try
            {
                RegistryKey root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

                RegistryKey subKey = root.OpenSubKey(watchDogPath);

                string value = (string)subKey.GetValue("schedule");

                return value;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace + ' ' + ex.Message);
            }
        }
        private void WriteToRegisry(string schedule)
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

        private void RestartService() {

            
            if (serviceController.Status.Equals(ServiceControllerStatus.Running) || serviceController.Status.Equals(ServiceControllerStatus.StartPending)){

                  serviceController.Stop();
            }

                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
                serviceController.Refresh();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            textBox1.Text = ReadFromRegistry();
            serviceController = new ServiceController("CheshkaWatchDog");
        }

        private void ADD_Click(object sender, EventArgs e) {
            string time = dateTimePicker1.Text;
            string currentText = textBox1.Text;
            string newText = currentText == "" ? time : $",{time}";
            textBox1.Text += newText;
        }

        private void CLEAR_Click(object sender, EventArgs e) {
            textBox1.Clear();
        }

        private void SaveRestart_Click(object sender, EventArgs e)
        {
            string schedule = textBox1.Text;

            try
            {

                progressBar1.Visible = !progressBar1.Visible;
                SaveRestart.Enabled = !SaveRestart.Enabled;
                Stop.Enabled = !Stop.Enabled;
                Start.Enabled = !Start.Enabled;
                WriteToRegisry(schedule);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            backgroundWorker1.RunWorkerAsync();
        }

        private void Start_Click(object sender, EventArgs e) {

            progressBar1.Visible = !progressBar1.Visible;
            SaveRestart.Enabled = !SaveRestart.Enabled;
            Stop.Enabled = !Stop.Enabled;
            Start.Enabled = !Start.Enabled;
            backgroundWorker2.RunWorkerAsync();
           
        }

        private void Stop_Click(object sender, EventArgs e) {

            progressBar1.Visible = !progressBar1.Visible;
            SaveRestart.Enabled = !SaveRestart.Enabled;
            Stop.Enabled = !Stop.Enabled;
            Start.Enabled = !Start.Enabled;
            backgroundWorker3.RunWorkerAsync();

        }

        private void Status_Click(object sender, EventArgs e)
        {
            string status = serviceController.Status.ToString();

            MessageBox.Show($"Status: {status}");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            RestartService();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {

                MessageBox.Show($"Can't restart service: {e.Error.Message}");
            }
            else
            {
                MessageBox.Show("Service successfully restarted!");
            }

            SaveRestart.Enabled = !SaveRestart.Enabled;
            Stop.Enabled = !Stop.Enabled;
            Start.Enabled = !Start.Enabled;
            progressBar1.Visible = !progressBar1.Visible;

        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

            if (serviceController.Status.Equals(ServiceControllerStatus.Running) | serviceController.Status.Equals(ServiceControllerStatus.StartPending))
            {
                throw new Exception("Service is already running!");
            }
           
            serviceController.Start();
            serviceController.WaitForStatus(ServiceControllerStatus.Running);
            serviceController.Refresh();
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {

                MessageBox.Show($"Can't start service: {e.Error.Message}");
            }
            else
            {
                MessageBox.Show("Service successfully started!");
            }

            SaveRestart.Enabled = !SaveRestart.Enabled;
            Stop.Enabled = !Stop.Enabled;
            Start.Enabled = !Start.Enabled;
            progressBar1.Visible = !progressBar1.Visible;

        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {

            if (serviceController.Status.Equals(ServiceControllerStatus.Stopped) | serviceController.Status.Equals(ServiceControllerStatus.StopPending))
            {
                throw new Exception("Service has been stopped or stoping at this monent!");
            }

            serviceController.Stop();
            serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
            serviceController.Refresh();
        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {

                MessageBox.Show($"Can't stop service: {e.Error.Message}");
            }
            else
            {
                MessageBox.Show("Service successfully stopped!");
            }

            SaveRestart.Enabled = !SaveRestart.Enabled;
            Stop.Enabled = !Stop.Enabled;
            Start.Enabled = !Start.Enabled;
            progressBar1.Visible = !progressBar1.Visible;

        }
    }
}
