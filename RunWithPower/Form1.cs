using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO; // Required for checking file existence

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

            if (comboBox1.Text == "Not chosen")
            {
                MessageBox.Show("Please choose an option!");
            }
            else if (comboBox1.Text == "Invoker")
            {
                arguments = $"-i \"{textBox1.Text}\""; // No special privileges for Invoker
                RunPsExec(arguments);
            }
            else if (comboBox1.Text == "Administrator")
            {
                arguments = $"-h \"{textBox1.Text}\""; // Run with Administrator privileges
                RunPsExec(arguments);
            }
            else if (comboBox1.Text == "System")
            {;
                RunPsExec(arguments);
            }
            else if (comboBox1.Text == "TrustedInstaller")
            {
                RunPsExec(arguments);
            }
            else
            {
                MessageBox.Show("Invalid");
            }
        }

        private void RunPsExec(string args)
        {
            try
            {
                // Check if PsExec.exe exists
                if (!File.Exists("PsExec.exe"))
                {
                    MessageBox.Show("Error: PsExec.exe not found in the application folder!");
                    return;
                }

                // Start PsExec.exe with arguments
                Process.Start("PsExec.exe", args);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running PsExec: {ex.Message}");
            }
        }
    }
}
