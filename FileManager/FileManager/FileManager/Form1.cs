using System;
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Cursach
{
    public partial class Form1 : Form
    {
        public string copyPath = "";
        public string copyRClick = "";
        public string FilePath = (Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf(@"Корневая папка\") + 14));
        public Stopwatch timer = new Stopwatch();
        public string dragStartPath = "";


        static public string logName="log.txt";

        public bool startFlash = false;

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            textBox1.Text = Environment.CurrentDirectory;
            showInListBox();
            Form5 f5 = new Form5();
            f5.Show();
            File.WriteAllText(Path.Combine(FilePath, logName), string.Empty);
        }


        //showInListBox-print dirs & files in listBox1
        private void showInListBox()
        {
            if (Directory.Exists(textBox1.Text))
            {

                listBox1.Items.Clear();

                DirectoryInfo dir = new DirectoryInfo(textBox1.Text);

                DirectoryInfo[] dirs = dir.GetDirectories(); // poluchem vse pappki iz dir
                foreach (DirectoryInfo currDir in dirs)       //add to list papki
                    listBox1.Items.Add(currDir);

                FileInfo[] files = dir.GetFiles();           //poluchaem vse file iz dir

                foreach (FileInfo currFile in files)          //add to list files
                    listBox1.Items.Add(currFile);
            }
            else
                textBox1.Text = FilePath;
        }

        //open-button1_click
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= FilePath.Length)
            {
                for (int i = 0; i < FilePath.Length; i++)
                {
                    if (textBox1.Text[i] != FilePath[i])
                        return;
                }
            }
            else
                return;
            if (Directory.Exists(textBox1.Text))
                showInListBox();
            else
            {
                textBox1.Text = FilePath;
                showInListBox();
            }

        }

        //select dir in listbox
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (startFlash)
                    startFlash = false;
                string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());

                if (Path.GetExtension(path) == "") //if dir
                {
                    textBox1.Text = path; //open selected 
                    showInListBox();
                }
                else                               //if extens
                {
                    Process.Start(path);
                    StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "System", logName), true);
                    sw.WriteLine($"Запущен {listBox1.SelectedItem}, время запуска : {DateTime.Now}");
                    sw.Close();
                }
            }
        }

        //back-button2_click
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == FilePath + "\\" || textBox1.Text == FilePath || (!startFlash && textBox1.Text.Length < FilePath.Length))
            {
                return;
            }
            if (textBox1.Text.Length > 3)
            {
                if (textBox1.Text[textBox1.Text.Length - 1] == '\\')
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
            }
            showInListBox();
        }

        //open in rklick
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
            if (Directory.Exists(path) || File.Exists(path))
            {
                if (Path.GetExtension(path) == "") //if dir
                {
                    textBox1.Text = path; //open selected 
                    showInListBox();
                }
                else                               //if extens
                {
                    Process.Start(path);
                    StreamWriter sw = new StreamWriter(Path.Combine(FilePath,"System",logName), true);
                    sw.WriteLine($"Запущен {listBox1.SelectedItem}, время запуска : {DateTime.Now}");
                    sw.Close();
                }
            }
            else
                textBox1.Text = FilePath;
        }

        //delete in rklick
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
            string trash = Path.Combine(Path.Combine(FilePath, "Корзина"), listBox1.SelectedItem.ToString());

            if (Directory.Exists(path) || File.Exists(path))
            {
                if (path != Path.Combine(FilePath, "System") && path != Path.Combine(FilePath, "Корзина") && textBox1.Text!= Path.Combine(FilePath, "System"))
                {
                    if (textBox1.Text == Path.Combine(FilePath, "Корзина"))
                    {
                        if (Path.GetExtension(path) == "")
                            Directory.Delete(path);
                        else
                            File.Delete(path);
                    }
                    else
                    {
                        if (Path.GetExtension(path) == "") //if dir
                        {
                            if (!Directory.Exists(trash))
                                Directory.Move(path, trash);
                            else
                                MessageBox.Show("Уже есть такой каталог, удаления не будет");
                        }
                        else                               //if extens
                        {
                            if (!File.Exists(trash))
                                File.Move(path, trash);
                            else
                                MessageBox.Show("Уже есть такой файл, удаления не будет");
                        }
                    }
                    showInListBox();
                }
                else
                    MessageBox.Show("Нет");
            }
            else
                textBox1.Text = FilePath;
        }

        //rename in rklick
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
            if (Directory.Exists(path) || File.Exists(path))
            {
                if (path != Path.Combine(FilePath, "System") && path != Path.Combine(FilePath, "Корзина"))
                {
                    Form2 f2 = new Form2();
                    f2.path = path;
                    f2.place = textBox1.Text;
                    if (Path.GetExtension(path) == "")
                        f2.isFile = false;
                    else
                        f2.isFile = true;
                    f2.Show();
                    showInListBox();
                }
                else
                    MessageBox.Show("Нет");
            }
            else
                textBox1.Text = FilePath;
        }

        //copy path
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            copyPath = textBox1.Text;
            pasteToolStripMenuItem1.Enabled = true;
        }

        //paste path
        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox1.Text = copyPath;
        }

        //clear path
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        //menu on rklick && start drag and drop
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (startFlash && e.Button == MouseButtons.Right)
                    startFlashMenu.Show(Cursor.Position.X, Cursor.Position.Y);
                else
                {
                    string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                    if (Directory.Exists(path) || File.Exists(path))
                    {
                        if (e.Button == MouseButtons.Left && path != Path.Combine(FilePath, "System") && path != Path.Combine(FilePath, "Корзина"))
                        {
                            timer.Start();
                            dragStartPath = path;
                        }
                        else
                        {

                            if (e.Button == MouseButtons.Right && path == Path.Combine(FilePath, "Корзина"))
                                menuTrash.Show(Cursor.Position.X, Cursor.Position.Y);
                            else
                            {
                                if (e.Button == MouseButtons.Right)
                                    menuListBoxItem.Show(Cursor.Position.X, Cursor.Position.Y);
                            }
                        }

                    }
                    else
                        textBox1.Text = FilePath;
                }
            }
            else
            {
                if (startFlash)
                    return;
                if (Directory.Exists(textBox1.Text))
                {
                    if (e.Button == MouseButtons.Right && listBox1.SelectedIndex == -1)
                        menuListBox.Show(Cursor.Position.X, Cursor.Position.Y);
                }
                else
                    textBox1.Text = FilePath;
            }
        }

        //func button
        private void button3_Click(object sender, EventArgs e)
        {
            if (startFlash)
                return;
            if (Directory.Exists(textBox1.Text))
            {
                if (textBox1.Text == Path.Combine(FilePath, "Корзина"))
                    menuFuncTrash.Show(Cursor.Position.X, Cursor.Position.Y);
                else
                    menuListBox.Show(Cursor.Position.X, Cursor.Position.Y);
            }
            else
                textBox1.Text = FilePath;
        }

        //copy in rklick
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
            if (Directory.Exists(path) || File.Exists(path))
            {
                copyRClick = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                pasteToolStripMenuItem.Enabled = true;
                PasteeToolStripMenuItem.Enabled = true;
            }
            else
                textBox1.Text = FilePath;
        }

        //pastee in func
        private void PasteeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
                copy(textBox1.Text);
            else
                textBox1.Text = FilePath;
            showInListBox();
        }

        //copy method
        void copy(string destinationPath)
        {
            if (copyRClick != destinationPath)
            {
                bool isfile;                                      //we copy file or dir?
                string fName = Path.GetFileName(copyRClick);
                bool exist = false;
                if (Path.GetExtension(copyRClick) == "")
                    isfile = false;
                else
                    isfile = true;


                DirectoryInfo dir = new DirectoryInfo(destinationPath);
                DirectoryInfo[] dirs = dir.GetDirectories();             //check folders names
                foreach (DirectoryInfo currDir in dirs)
                    if (currDir.Name == fName)
                        exist = true;

                FileInfo[] files = dir.GetFiles();                        //check file names
                foreach (FileInfo currFile in files)
                    if (currFile.Name == fName)
                        exist = true;

                if (!exist)                                               //if copy name is unique
                {
                    if (isfile)
                    {
                        if (File.Exists(copyRClick))
                            File.Copy(copyRClick, Path.Combine(destinationPath, fName));
                        else
                            MessageBox.Show("Вы сделали что-то не так");
                    }
                    else
                    {
                        if (Directory.Exists(copyRClick))
                            CopyDir(copyRClick, Path.Combine(destinationPath, fName));
                        else
                            MessageBox.Show("Вы сделали что-то не так");
                    }
                }
                else                                                       //if copy name isn't unique
                    MessageBox.Show("Такое имя уже существует");
            }
            else
                MessageBox.Show("Нет");
        }

        //recursion to copy folder & everything inside it
        void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
                CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
        }

        //paste in rklick
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
            if (Directory.Exists(path))
            {
                if (Path.GetExtension(path) == "")                     //we copy in folder
                    copy(path);
                else                                                    //we copy in file???
                    MessageBox.Show("Куда копируешь:?");
            }
            else
                textBox1.Text = FilePath;
        }

        //open trash
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
        }

        //clear trash
        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Directory.Delete(Path.Combine(FilePath, "Корзина"), true);
            Directory.CreateDirectory(Path.Combine(FilePath, "Корзина"));
        }

        //clear trash in func
        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dToolStripMenuItem_Click(sender, e);
        }

        //add new folder in func
        private void рToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (Directory.Exists(path))
            {
                Form2 f2 = new Form2();
                f2.path = path;
                f2.place = path;
                f2.isFile = false;
                f2.operationCreate = true;

                f2.Show();
            }
            else
                textBox1.Text = FilePath;
        }

        //add new file in func
        private void пToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (Directory.Exists(path))
            {
                Form2 f2 = new Form2();
                f2.path = path;
                f2.place = path;
                f2.isFile = true;
                f2.operationCreate = true;

                f2.Show();
            }
            else
                textBox1.Text = FilePath;
        }

        //utilities-управление комьютером
        private void управлениеКомпьютеромToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Windows\\system32\\compmgmt.msc");
            StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "System", logName), true);
            sw.WriteLine($"Запущен compmgmt.msc, время запуска : {DateTime.Now}");
            sw.Close();
        }

        //utilities-командная строка
        private void команднаяСтрокаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Windows\\system32\\cmd.exe");
            StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "System", logName), true);
            sw.WriteLine($"Запущен cmd.exe, время запуска : {DateTime.Now}");
            sw.Close();
        }

        //utilities-мониторинг ресурсов
        private void мониторингРесурсовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Windows\\system32\\perfmon.exe");
            StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "System", logName), true);
            sw.WriteLine($"Запущен perfmon.exe, время запуска : {DateTime.Now}");
            sw.Close();
        }

        //utilities-сведения о системе
        private void сведенияОСистемеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Windows\\system32\\msinfo32.exe");
            StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "System", logName), true);
            sw.WriteLine($"Запущен msinfo32.exe, время запуска : {DateTime.Now}");
            sw.Close();
        }

        //hotKeys
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
                listBox1_MouseDoubleClick(listBox1, null);

            if (e.KeyValue == (char)Keys.Escape)
                Close();
            if (e.Modifiers == Keys.Shift && e.KeyValue == (char)Keys.Delete && listBox1.SelectedIndex != -1)
            {
                string path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                if (path != Path.Combine(FilePath, "System") && path != Path.Combine(FilePath, "Корзина") && textBox1.Text != Path.Combine(FilePath, "System"))
                {
                    if (Path.GetExtension(path) == "") //if dir
                    {
                        try
                        {
                            Directory.Delete(path);
                        }
                        catch
                        {
                            File.Delete(path);
                        }
                    }
                    else                               //if extens
                        File.Delete(path);
                }
                showInListBox();
            }

            if (e.KeyValue == (char)Keys.Delete)
            {
                if (listBox1.SelectedIndex != -1)
                    toolStripMenuItem2_Click(toolStripMenuItem2, null);
            }

            int index = listBox1.SelectedIndex;
            if (e.KeyValue == (char)Keys.C && e.Modifiers == Keys.Control)
            {
                if (listBox1.SelectedIndex != -1)
                    copyToolStripMenuItem_Click(copyToolStripMenuItem, null);
            }

            if (e.KeyValue == (char)Keys.V && e.Modifiers == Keys.Control)
            {
                if (listBox1.SelectedIndex != -1)
                    pasteToolStripMenuItem_Click(pasteToolStripMenuItem, null);
                else
                    PasteeToolStripMenuItem_Click(PasteeToolStripMenuItem, null);
            }

        }

        //drag and drop
        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && timer.ElapsedMilliseconds > 500)
            {
                string pathTo = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                string lastpath = Path.Combine(pathTo, Path.GetFileName(dragStartPath));
                if (Directory.Exists(pathTo))
                {
                    if (Path.GetExtension(dragStartPath) == "") //if dir
                    {
                        if (!Directory.Exists(lastpath) && pathTo != dragStartPath)
                            Directory.Move(dragStartPath, lastpath);
                        else
                            MessageBox.Show("Уже есть такой каталог, перемещения не будет");
                    }
                    else                               //if extens
                    {
                        if (!File.Exists(lastpath))
                            File.Move(dragStartPath, lastpath);
                        else
                            MessageBox.Show("Уже есть такой файл, перемещения не будет");

                    }
                    showInListBox();
                }
                else
                    MessageBox.Show("Неверное место для копирования");
            }
            timer.Reset();
        }

        //terminal linux
        private void linuxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(Environment.UserName + "@" + Environment.MachineName.ToString() + ":~$");
            f3.isLinux = true;
            f3.bumper = Environment.UserName + "@" + Environment.MachineName.ToString() + ":~$";
            f3.Show();
        }

        //terminal windows
        private void windowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3("C:\\Users\\" + Environment.UserName + ">");
            f3.isLinux = false;
            f3.bumper = "C:\\Users\\" + Environment.UserName + ">";
            f3.Show();

        }

        //go to flash
        private void goToFlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            bool exists = false;
            foreach (var dInfo in DriveInfo.GetDrives())
            {
                if (dInfo.IsReady && dInfo.DriveType == DriveType.Removable)
                {
                    listBox1.Items.Add(dInfo.Name);
                    exists = true;
                }
            }
            if (!exists)
            {
                MessageBox.Show("Нет ЮСБ");
                goToToolStripMenuItem_Click(sender, e);
            }
            else
                startFlash = true;
        }

        //go to root
        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = FilePath+"\\";
            showInListBox();
            startFlash = false;
        }

        //open flash
        private void onlyOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = listBox1.SelectedItem.ToString();
            if (Directory.Exists(path))
            {
                textBox1.Text = path; //open selected 
                showInListBox();
                startFlash = false;
            }
        }

        //kommands terminal
        private void командыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Linux:\n\tps-показазать все процессы\n\tkill <id>-убить процесс с id <id>" +
                "\n\tfree-показать сколько памяти занимает процесс\n\tpgrep <name>-получить id процесса с именем <name>" +
                "\n\tuptime-время работы компьютера\n\ttimes-время работы процессов\n\tdf-просмотр информации о дисках" +
                "\n\tfdisk-просмотр дисков и разделов компьютера\n\tnotepad.exe-запуск блокнота" +
                "\n\tchrome.exe-запуск хрома" +
                "" +
                "\nWindows:\n\ttasklist-показазать все процессы\n\ttaskkill <id>-убить процесс с id <id>" +
                "\n\tfree-показать сколько памяти занимает процесс\n\tpgrep <name>-получить id процесса с именем <name>" +
                "\n\tnet_statistics_workstation-время работы компьютера\n\ttimes-время работы процессов\n\tDISKPART_list_volume-просмотр информации о дисках" +
                "\n\twmic_logicaldisk_get_description,name-просмотр дисков и разделов компьютера\n\tstart_notepad.exe-запуск блокнота" +
                "\n\tstart_chrome.exe-запуск хрома");
        }

        //inside
        private void процессыВнутриФМToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }


        Semaphore sem = new Semaphore(1, 1);


        public void total()
        {
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName == "Prilojenie")
                {
                    MessageBox.Show("Приложение уже запущено");
                    return;
                }
            }
            File.WriteAllText(Path.Combine(FilePath,"ind.txt"), String.Empty);
            sem.WaitOne();
            zad13();
            sem.Release();
            sem.WaitOne();
            zad17();
            sem.Release();
            sem.WaitOne();
            zad20();
            sem.Release();
            Process.Start(Path.Combine(FilePath,"System\\Prilojenie.exe"));
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(FilePath, "ind.txt"), String.Empty);

            zad13();

            zad17();

            zad20();

        }

        public void zad13()
        {
            StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "ind.txt"), true);
            sw.WriteLine("13)загрузка каждого ядра ЦП в %");
            sw.Close();
        }
        public void zad17()
        {
            StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "ind.txt"), true);
            sw.WriteLine("Ширинa:" + Width.ToString() + "и Высота:" + Height.ToString() + " рамки окна");
            sw.Close();
        }
        public void zad20()
        {
            StreamWriter sw = new StreamWriter(Path.Combine(FilePath, "ind.txt"), true);
            try
            {
                string s = "";
                Process process = Process.GetCurrentProcess();
                ProcessModuleCollection modules = process.Modules;
                foreach (ProcessModule module in modules)
                    s += $"Name: {module.ModuleName}  \t\t FileName: {module.FileName}\n";
                sw.WriteLine(s);
            }
            catch
            {
            }
            sw.Close();
        }

        private void functionalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            total();
        }
    }
}