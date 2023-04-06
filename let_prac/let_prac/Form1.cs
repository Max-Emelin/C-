using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace let_prac
{
    public partial class Form1 : Form
    {
        Dictionary<String, table> myMap = new Dictionary<String, table>();

        List<string> combinationsOfQuantities = new List<string>();

        public int count = 1;
        public string[] txt;

        public string topValue;
        public string[] changedTxt;
        
        public Form1()
        {
            InitializeComponent();
            Add();
        }
        public void Add()
        {
            table F = new table(1, 1, -2);
            myMap.Add("F", F);

            table a = new table(0, 1, -2);
            myMap.Add("a", a);

            table m = new table(1, 0, 0);
            myMap.Add("m", m);

            table A = new table(1, 2, -2);
            myMap.Add("A", A);

            table P = new table(1, 2, -3);
            myMap.Add("P", P);

            table p = new table(1, -1, -2);
            myMap.Add("p", p);

            table L = new table(0, 1, 0);
            myMap.Add("L", L);

            table V = new table(0, 1, -1);
            myMap.Add("V", V);

            table t = new table(0, 0, 1);
            myMap.Add("t", t);
        }
        public struct table
        {
            public int kg;
            public int m;
            public int s;
            public int A;//A-amper
            public int K;
            public int mol;
            public int cd;//cd-kandela(power of light)
            public table(int kg, int m, int s)
            {
                this.kg = kg;
                this.m = m;
                this.s = s;
                this.A = 0;
                this.K = 0;
                this.mol = 0;
                this.cd = 0;
            }
            public table(int kg, int m, int s, int A, int K, int mol, int cd)
            {
                this.kg = kg;
                this.m = m;
                this.s = s;
                this.A = A;
                this.K = K;
                this.mol = mol;
                this.cd = cd;
            }
        }

        public void printPowerset(int n)
        {
            int[] stack = new int[40];
            int k = 0;
            stack[0] = 0;
            k = 0;
            while (true)
            {
                if (stack[k] < n)
                {
                    stack[k + 1] = stack[k] + 1;
                    k++;
                }
                else
                {
                    stack[k - 1]++;
                    k--;
                }
                if (k == 0)
                    break;
                if (k <= 7)
                    createMatrix(stack, k);
            }
            return;
        }
        private void findBTN_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear();
            count = 1;
            if (dependentTextBox.Text == string.Empty || componentsTextBox.Text == string.Empty)
            {
                dependentTextBox.Text = "F";
                componentsTextBox.Text = "a m A t p L P";
            }
            txt = componentsTextBox.Text.ToString().Split();

            changedTxt = componentsTextBox.Text.ToString().Split();
            topValue = dependentTextBox.Text;

            printPowerset(txt.Length);
            swapMagnitude();

        }

        public void swapMagnitude()
        {
            topValue = dependentTextBox.Text;
            changedTxt = componentsTextBox.Text.ToString().Split();
            printPowerset(txt.Length);
            for(int i=0;i<txt.Length;i++)
            {
                topValue = txt[i];
                changedTxt[i] = dependentTextBox.Text;
                printPowerset(txt.Length);
                changedTxt[i] = txt[i];
            }
           
        }

        public void createMatrix(int[] arrOfIndex, int size)
        {
            bool[] a = new bool[7];

            //schet ne nulevix znach v zavisimix
            for (int i = 1; i <= size; i++)
            {
                table t;
         
                t = myMap[changedTxt[arrOfIndex[i] - 1]];
                if (t.kg != 0) a[0] = true;
                if (t.m != 0) a[1] = true;
                if (t.s != 0) a[2] = true;
                if (t.A != 0) a[3] = true;
                if (t.K != 0) a[4] = true;
                if (t.mol != 0) a[5] = true;
                if (t.cd != 0) a[6] = true;
            }
            //schet ne nulevix znach v osnovnoy
            for (; ; )
            {
                table tt = myMap[topValue];
                if (tt.kg != 0) a[0] = true;
                if (tt.m != 0) a[1] = true;
                if (tt.s != 0) a[2] = true;
                if (tt.A != 0) a[3] = true;
                if (tt.K != 0) a[4] = true;
                if (tt.mol != 0) a[5] = true;
                if (tt.cd != 0) a[6] = true;
                break;
            }


            int countString = 0;
            for (int i = 0; i < 7; i++)
                if (a[i]) countString++;


            if (countString < size)
                return;


            double[,] arr = new double[countString, size];
            double[] x = new double[countString];
            //zapolnenie arr
            for (int i = 0; i < size; i++)
            {
                int j = 0;
                table t = myMap[changedTxt[arrOfIndex[i + 1] - 1]];
                if (a[0])
                {
                    arr[j, i] = t.kg;
                    j++;
                }
                if (a[1])
                {
                    arr[j, i] = t.m;
                    j++;
                }
                if (a[2])
                {
                    arr[j, i] = t.s;
                    j++;
                }
                if (a[3])
                {
                    arr[j, i] = t.A;
                    j++;
                }
                if (a[4])
                {
                    arr[j, i] = t.K;
                    j++;
                }
                if (a[5])
                {
                    arr[j, i] = t.mol;
                    j++;
                }
                if (a[6])
                {
                    arr[j, i] = t.cd;
                    j++;
                }
            }
            //zapolnenie x
            for (; ; )
            {
                table tabX = myMap[topValue];
                int j = 0;
                if (a[0])
                {
                    x[j] = tabX.kg;
                    j++;
                }
                if (a[1])
                {
                    x[j] = tabX.m;
                    j++;
                }
                if (a[2])
                {
                    x[j] = tabX.s;
                    j++;
                }
                if (a[3])
                {
                    x[j] = tabX.A;
                    j++;
                }
                if (a[4])
                {
                    x[j] = tabX.K;
                    j++;
                }
                if (a[5])
                {
                    x[j] = tabX.mol;
                    j++;
                }
                if (a[6])
                {
                    x[j] = tabX.cd;
                }
                break;
            }


            double[,] copyArr = new double[countString, size];
            double[] copyX = new double[countString];
            //zapolnenie copy
            for (int i = 0; i< countString;i++)
            {
                for (int j = 0; j < size; j++)
                    copyArr[i, j] = arr[i, j];
                copyX[i] = x[i];
            }
                    



            string s = "";



            if (size < countString)
            {
                double[,] dd = new double[size, size];
                double[] yy = new double[size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                        dd[i, j] = arr[i, j];
                    yy[i] = x[i];
                }
                double[] anss = gauss(size, dd, yy);

                if (double.IsNaN(anss[0]))
                    return;

                else
                {
                    //проверка для всех строк
                    for (int i = 0; i < countString; i++)
                    {
                        double sumRow = 0;
                        for (int j = 0; j < size; j++)
                            sumRow += Math.Pow(copyArr[i, j], anss[j]);
                        if (Math.Abs(sumRow - copyX[i]) > 0.0001)
                            return;
                    }

                    //печать матриц
                   /* string str = "";
                    for (int h=0;h<countString;h++)
                    {
                        for (int k = 0; k < size; k++)
                            str += copyArr[h, k] + " ";

                        str += "\t\t" + copyX[h];

                        listBox1.Items.Add(str + "\n");
                        str = "";
                    } */

                    //печать ответа
                    s = "";
                    for (int i = 0; i < size; i++)
                        s += anss[i] + " ";
                    formula(arrOfIndex, size, anss);

                   // listBox1.Items.Add("-------------------------------");
                }
            }
            else
            {
                double[] ans = gauss(countString, arr, x);
                if(double.IsNaN(ans[0]))
                    return;
                
                //проверка на эпсилоны+ составление строки для печати 
                s = "";
                for (int i = 0; i < countString; i++)
                {
                    s += ans[i] + " ";
                    if (Math.Abs(ans[i]) <= 0.001 && Math.Abs(ans[i]) > 0)
                        return;
                }

                //проверка для всех строк
                

                //печать матриц
               /* string str = "";
                for (int h = 0; h < countString; h++)
                {
                    for (int k = 0; k < size; k++)
                        str += copyArr[h, k] + " ";

                    str += "\t\t" + copyX[h];

                    listBox1.Items.Add(str + "\n");
                    str = "";
                }*/


                formula(arrOfIndex, size, ans);
            }
        }

        public double[] gauss(int countString,double[,] a,double[] y)
        {
            double[] x = new double[countString];
            double max;
            int k, index;
            const double eps = 1;  // точность
            k = 0;
            while (k < countString)
            {
                // Поиск строки с максимальным a[i][k]
                max = Math.Abs(a[k,k]);
                index = k;
                for (int i = k + 1; i < countString; i++)
                {
                    if (Math.Abs(a[i,k]) > max)
                    {
                        max = Math.Abs(a[i,k]);
                        index = i;
                    }
                }
                // Перестановка строк
                if (max < eps)
                {
                    // нет ненулевых диагональных элементов
                    for (int i = 0; i < countString; i++)
                        x[i] = double.NaN;
                    return x;
                }
                for (int j = 0; j < countString; j++)
                {
                    double tempp= a[k,j];
                    a[k,j] = a[index,j];
                    a[index,j] = tempp;
                }
                double temp = y[k];
                y[k] = y[index];
                y[index] = temp;
                // Нормализация уравнений
                for (int i = k; i < countString; i++)
                {
                    double tempp = a[i,k];
                    if (Math.Abs(tempp) < eps) continue; // для нулевого коэффициента пропустить
                    for (int j = 0; j < countString; j++)
                        a[i,j] = a[i,j] / tempp;
                    y[i] = y[i] / tempp;
                    if (i == k) continue; // уравнение не вычитать само из себя
                    for (int j = 0; j < countString; j++)
                        a[i,j] = a[i,j] - a[k,j];
                    y[i] = y[i] - y[k];
                }
                k++;
            }
            // обратная подстановка
            for (k = countString - 1; k >= 0; k--)
            {
                x[k] = y[k];
                for (int i = 0; i < k; i++)
                    y[i] = y[i] - a[i,k] * x[k];
            }
            return x;
        }

        public void formula(int[] arrOfIndex, int size, double[] ans)
        {
            int cc = 0;
            for (int i=0;i<size; i++)
                if (ans[i] != 0)
                    cc++;

            char[] characters = new char[cc+1];
            for (int i = 0,  j=0; i < size;i++)
            {
                if(ans[i]!=0)
                {
                    characters[j] = Convert.ToChar(changedTxt[arrOfIndex[i + 1] - 1]);
                    j++;
                }
            }
            characters[cc] = Convert.ToChar(topValue);

            Array.Sort(characters);

            string q = "";
            for (int i = 0; i <= cc; i++)
                q += characters[i];

            string testExists = "";
            testExists = combinationsOfQuantities.Find(x => x.Contains(q));

            if (testExists == null)
            {
                combinationsOfQuantities.Add(q);
                string s = count + ") " + topValue + "^-1";
                count++;
                for (int i = 1; i <= size; i++)
                {
                    if (ans[i - 1] != 0)
                    {
                        s += " * ";
                        s += changedTxt[arrOfIndex[i] - 1] + "^" + ans[i - 1];
                    }
                }
                resultsListBox.Items.Add(s);
                resultsListBox.Items.Add("-------------------------------");
            }
        }

       
    }
}
