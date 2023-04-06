using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursach
{
    public partial class Form3 : Form
    {
        public string action;
        public bool isLinux = true;
        public string bumper = "";

        public Form3()
        {
            InitializeComponent();
            richTextBox1.AppendText(bumper);
        }
        public Form3(string b)
        {
            InitializeComponent();
            richTextBox1.AppendText(b);
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (richTextBox1.SelectedText != "")
            {
                e.SuppressKeyPress = true;
            }

            string[] line = richTextBox1.Lines;
            int len = richTextBox1.Lines.Length - 1;
            if (e.KeyCode == Keys.Enter)
            {
                action = line[len];
                e.SuppressKeyPress = true;
                starting();
            }
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                if (bumper == line[len])
                    e.SuppressKeyPress = true;
            }
        }
        
        private void starting()
        {
            if (isLinux)
                action = action.Substring(action.LastIndexOf("$") + 1, action.Length - action.LastIndexOf("$") - 1);
            else
                action = action.Substring(action.LastIndexOf(">") + 1, action.Length - action.LastIndexOf(">") - 1);

            string[] actioncmd = action.Split(new Char[] { ' ' });

            int index = action.LastIndexOf(" ");
            if (index > 0)
            {
                action = action.Substring(0, index);
            }
            try
            {
                if (actioncmd[0] == "ps" && isLinux || actioncmd[0] == "tasklist" && !isLinux)
                {
                    foreach (Process process in Process.GetProcesses())
                    {
                        try
                        {
                            richTextBox1.AppendText("\n\t " + process.Id.ToString() +
                                "\t\t" + process.ProcessName +
                                "\t\t" + process.StartTime +
                                "\t\t" + process.PriorityClass);
                        }
                        catch
                        {
                        }
                    }
                }

                if (actioncmd[0] == "kill" && isLinux || actioncmd[0] == "taskkill" && !isLinux)
                {
                    foreach (Process process in Process.GetProcesses())
                    {
                        if (actioncmd[1] == process.Id.ToString())
                            process.Kill();
                        //test w0 [1] before with [1]
                    }
                }

                if (actioncmd[0] == "free" && isLinux || actioncmd[0] == "free" && !isLinux)
                {
                    foreach (Process process in Process.GetProcesses())
                        richTextBox1.AppendText("\n" + process.ProcessName + "\t" + (process.WorkingSet64 / 1024 / 1024) + "МБ");
                }

                if (actioncmd[0] == "pgrep" && isLinux || actioncmd[0] == "pgrep" && !isLinux)
                {
                    foreach (Process process in Process.GetProcesses())
                    {
                        if (actioncmd[1] == process.ProcessName)
                            richTextBox1.AppendText("\n\t" + actioncmd[1] + "->" + process.Id);
                    }
                }
                if (actioncmd[0] == "uptime" && isLinux || actioncmd[0] == "net_statistics_workstation" && !isLinux)
                {
                    var pc = new PerformanceCounter("System", "System Up Time");
                    pc.NextValue();
                    richTextBox1.AppendText("\n" + TimeSpan.FromSeconds(pc.NextValue()));
                }

                if (actioncmd[0] == "times" && isLinux || actioncmd[0] == "times" && !isLinux)
                {
                    foreach (Process process in Process.GetProcesses())
                    {
                        try
                        {
                            richTextBox1.AppendText("\n\t " + process.ProcessName +
                                "\t\t" + (DateTime.Now - process.StartTime).TotalSeconds);
                        }
                        catch
                        {
                        }
                    }
                }
                if (actioncmd[0] == "df" && isLinux || actioncmd[0] == "DISKPART_list_volume" && !isLinux)
                {
                    DriveInfo[] allDrives = DriveInfo.GetDrives();
                    foreach (DriveInfo d in allDrives)
                        richTextBox1.AppendText("\n" + d.Name +
                            "\t" + d.DriveType + "\t" + d.DriveFormat +
                            "\tfree:" + d.TotalFreeSpace + "\ttotal:" + d.TotalSize);
                    
                }
                if (actioncmd[0] == "fdisk" && isLinux || actioncmd[0] == "wmic_logicaldisk_get_description,name" && !isLinux)
                {
                    DriveInfo[] allDrivess = DriveInfo.GetDrives();
                    foreach (DriveInfo d in allDrivess)
                    {
                        richTextBox1.AppendText("\nДиск: " + d.Name +
                            "\n\tТип диска: " + d.DriveType + "\n\tФайловая система: " + d.DriveFormat +
                            "\n\tСвободно: " + d.TotalFreeSpace / 100000 / 10000 + " ГБ\n\tРазмер: " + d.TotalSize / 100000 / 10000 + " ГБ");
                    }
                }
                //8^
                if (actioncmd[0] == "notepad.exe" && isLinux || actioncmd[0] == "start_notepad.exe" && !isLinux)
                {
                    Process.Start("C:\\Windows\\system32\\notepad.exe");
                }
                if (actioncmd[0] == "chrome.exe" && isLinux || actioncmd[0] == "start_chrome.exe" && !isLinux)
                {
                    Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe");
                }

                    richTextBox1.AppendText("\n"+bumper);
            }
            catch
            {
                richTextBox1.AppendText("\nTRY AGAIN!\n");
                richTextBox1.AppendText(bumper);
            }
        }


    }
}
