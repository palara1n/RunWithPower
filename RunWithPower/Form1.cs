using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace RunWithPower
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string arguments = $"-s -i \"{textBox1.Text}\"";

            switch (comboBox1.Text)
            {
                case "Not chosen":
                    MessageBox.Show("Please choose an option!");
                    break;

                case "Invoker":
                    arguments = $"-i \"{textBox1.Text}\""; // No special privileges for Invoker
                    RunPsExec(arguments);
                    break;

                case "Administrator":
                    arguments = $"-h \"{textBox1.Text}\""; // Run with Administrator privileges
                    RunPsExec(arguments);
                    break;

                case "System":
                    RunPsExec(arguments);
                    break;

                case "TrustedInstaller":
                    RunAsTrustedInstaller(textBox1.Text);
                    break;

                default:
                    MessageBox.Show("Invalid option selected.");
                    break;
            }
        }

        private void RunPsExec(string args)
        {
            try
            {
                if (!File.Exists("PsExec.exe"))
                {
                    MessageBox.Show("Error: PsExec.exe not found in the application folder!");
                    return;
                }

                Process.Start("PsExec.exe", args);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running PsExec: {ex.Message}");
            }
        }

        private void RunAsTrustedInstaller(string applicationPath)
        {
            try
            {
                ServiceController sc = new ServiceController("TrustedInstaller");

                if (sc.Status != ServiceControllerStatus.Running)
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                }

                Process[] proc = Process.GetProcessesByName("TrustedInstaller");

                if (proc.Length > 0)
                {
                    IamYourDaddy.Run(proc[0].Id, applicationPath);
                }
                else
                {
                    MessageBox.Show("Error: TrustedInstaller process not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running as TrustedInstaller: {ex.Message}");
            }
        }
    }
}
