using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursach
{
    public partial class Form2 : Form
    {
        public string path;
        public string place;
        public bool isFile;
        public bool operationCreate = false;
        public Form2()
        {
            InitializeComponent();
        }

        //cancle
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        //confirm
        private void button2_Click(object sender, EventArgs e)
        {
            bool exist = false;
           
            DirectoryInfo dir = new DirectoryInfo(place);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo currDir in dirs)
                if (currDir.Name == textBox1.Text)
                    exist = true;

            FileInfo[] files = dir.GetFiles();           
            foreach (FileInfo currFile in files)
                if (currFile.Name == textBox1.Text)
                    exist = true;

            if (!exist)
            {
                if (!operationCreate)
                {
                    if (isFile)
                        File.Move(path, Path.Combine(place, textBox1.Text));
                    else
                        Directory.Move(path, Path.Combine(place, textBox1.Text));
                }
                else
                {
                    if (isFile)
                        File.Create(Path.Combine(path, textBox1.Text));
                    else
                        Directory.CreateDirectory(Path.Combine(path, textBox1.Text));
                }
            }
            else
                MessageBox.Show("Такое имя уже есть, я ничего не делаю");
            Close();
        }
    }
}
