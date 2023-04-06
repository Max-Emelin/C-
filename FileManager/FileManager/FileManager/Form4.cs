using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursach
{
    public partial class Form4 : Form
    {
        public string FilePath = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf(@"Корневая папка\") + 14);
        public Form4()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(FilePath, "System", Form1.logName)))
            {
                richTextBox1.Clear();
                StreamReader sr = new StreamReader(Path.Combine(FilePath, "System", Form1.logName));
                richTextBox1.AppendText(sr.ReadToEnd());
                sr.Close();
            }

            
        }
    }
}
