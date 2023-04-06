using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Management;

namespace Prilojenie
{
    public partial class Povariantu : Form
    {
        public string FilePath = (Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf(@"Корневая папка\") + 14));
        public Povariantu()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader(Path.Combine(FilePath, "ind.txt"));
            richTextBox1.AppendText(sr.ReadToEnd());
            sr.Close();
            timer1.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(Path.Combine(FilePath, "ind.txt"));
            richTextBox1.Clear();
            richTextBox1.AppendText(sr.ReadToEnd());
            sr.Close();
        }
    }
}
