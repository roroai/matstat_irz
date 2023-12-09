using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace matstat_irz
{
    public partial class MainForm : Form
    {
        public double[] xarray, yarray;
        public double xmax, ymax, xmin, ymin, xr, yr, xh, yh;

        public double xslash, xs, yslash, ys;
        public MainForm()
        {
            InitializeComponent();
        }

        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            if(inputBox.Text.Length > 0)
            {
                inputButton.Enabled = true;
            }
        }

        private void inputButton_Click(object sender, EventArgs e)
        {
            int n;
            String[] ss = inputBox.Text.Split(' ');
            n = ss.Count();
            double[] array = new double[n];
            for (int i = 0; i < n; i++)
            {
                array[i] = double.Parse(ss[i]);
            }
            if(n != 100)
            {
                MessageBox.Show("Некорректный ввод", "Повторите ввод",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    inputDataGridView.Rows.Add(array[i], array[i+1]);
                    i++;
                }
                inputButton.Enabled = false;
                inputBox.Enabled = false;
                button1.Enabled = true;
            }
        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44 && number != 32) 
            {
                e.Handled = true;
            }
        }

        // Составление группированных рядов
        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            groupBox1.Visible = true;


            // Поиск мин, макс и размаха х и у
            xarray = new double[inputDataGridView.RowCount];
            yarray = new double[inputDataGridView.RowCount];
            for(int i = 0; i < inputDataGridView.RowCount; i++)
            {
                xarray[i] = double.Parse(inputDataGridView[0, i].Value.ToString());
                yarray[i] = double.Parse(inputDataGridView[1, i].Value.ToString());
            }
            xmax = xarray.Max();
            ymax = yarray.Max();
            xmin = xarray.Min();
            ymin = yarray.Min();
            xr = xmax- xmin;
            yr = ymax- ymin;
            xh = (xr / 7) + 1;
            yh = (yr / 7) + 1;


            xlabel.Text = "Xmax = " + xmax.ToString() + "   Xmin = " + xmin.ToString() + "   Rx = " + xr.ToString();
            ylabel.Text = "Ymax = " + ymax.ToString() + "   Ymin = " + ymin.ToString() + "   Yx = " + yr.ToString();
            xhlabel.Text = "hx = " + ((int)xh).ToString();
            yhlabel.Text = "hy = " + ((int)yh).ToString();

            //Таблица для х
            int[] xn = { 0, 0, 0, 0, 0, 0, 0 };
            //Подсчет  n-iтых (попадание в промежуток)
            for(int i = 0; i < xarray.Length; i++)
            {
                if(xarray[i] < (int)xmin + (int)xh)
                {
                    xn[0]++;
                }else if(xarray[i] >= (int)xmin + (int)xh && xarray[i] < (int)xmin + 2 * (int)xh)
                {
                    xn[1]++;
                }else if (xarray[i] >= (int)xmin + 2 * (int)xh && xarray[i] < (int)xmin + 3 * (int)xh)
                {
                    xn[2]++;
                }else if (xarray[i] >= (int)xmin + 3 * (int)xh && xarray[i] < (int)xmin + 4 * (int)xh)
                {
                    xn[3]++;
                }else if (xarray[i] >= (int)xmin + 4 * (int)xh && xarray[i] < (int)xmin + 5 * (int)xh)
                {
                    xn[4]++;
                }else if(xarray[i] >= (int)xmin + 5 * (int)xh && xarray[i] < (int)xmin + 6 * (int)xh)
                {
                    xn[5]++;
                }else
                {
                    xn[6]++;
                }
            }

            //вывод таблицы для х
            for (int i = 0; i < xn.Length; i++)
            {
                xdataGridView.Rows.Add("["+((int)xmin + (int)xh * i).ToString() + ", " + ((int)xmin + (int)xh * i + (int)xh).ToString() +"]",
                    (double)((int)xmin + (int)xh * i+ (int)xmin + (int)xh * i + (int)xh)/2,
                    xn[i],
                    (double)xn[i]/50,
                    (double)xn[i]/(50*(int)xh));
            }

            // Диаграммы для х
            xchart1.Series[0].Points.Clear();
            xchart2.Series[0].Points.Clear();
            for (int i = 0; i < xn.Length; i++)
            {
                xchart1.Series[0].Points.AddXY(xdataGridView[1, i].Value, xdataGridView[3, i].Value);
                xchart2.Series[0].Points.AddXY(xdataGridView[0, i].Value, xdataGridView[4, i].Value);
            }

            //Таблица для y
            int[] yn = { 0, 0, 0, 0, 0, 0, 0 };
            //Подсчет  m-iтых (попадание в промежуток)
            for (int i = 0; i < yarray.Length; i++)
            {
                if (yarray[i] < (int)ymin + (int)yh)
                {
                    yn[0]++;
                }
                else if (yarray[i] >= (int)ymin + (int)yh && yarray[i] < (int)ymin + 2 * (int)yh)
                {
                    yn[1]++;
                }
                else if (yarray[i] >= (int)ymin + 2 * (int)yh && yarray[i] < (int)ymin + 3 * (int)yh)
                {
                    yn[2]++;
                }
                else if (yarray[i] >= (int)ymin + 3 * (int)yh && yarray[i] < (int)ymin + 4 * (int)yh)
                {
                    yn[3]++;
                }
                else if (yarray[i] >= (int)ymin + 4 * (int)yh && yarray[i] < (int)ymin + 5 * (int)yh)
                {
                    yn[4]++;
                }
                else if (yarray[i] >= (int)ymin + 5 * (int)yh && yarray[i] < (int)ymin + 6 * (int)yh)
                {
                    yn[5]++;
                }
                else
                {
                    yn[6]++;
                }
            }

            //вывод таблицы для y
            for (int i = 0; i < yn.Length; i++)
            {
                ydataGridView.Rows.Add("[" + ((int)ymin + (int)yh * i).ToString() + ", " + ((int)ymin + (int)yh * i + (int)yh).ToString() + "]",
                    (double)((int)ymin + (int)yh * i + (int)ymin + (int)yh * i + (int)yh) / 2,
                    yn[i],
                    (double)yn[i] / 50,
                    (double)yn[i] / (50 * (int)yh));
            }

            // Диаграммы для y
            ychart1.Series[0].Points.Clear();
            ychart2.Series[0].Points.Clear();
            for (int i = 0; i < yn.Length; i++)
            {
                ychart1.Series[0].Points.AddXY(ydataGridView[1, i].Value, ydataGridView[3, i].Value);
                ychart2.Series[0].Points.AddXY(ydataGridView[0, i].Value, ydataGridView[4, i].Value);
            }
            button2.Enabled = true;
            button1.Enabled = false;
        }

        // Вычисление точечных оценок
        private void button2_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            tabControl1.SelectedTab = tabPage2;
            button2.Enabled=false;

            //Создание таблицы
            for(int i = 0; i < 7; i++)
            {
                dataGridViewTask2.Rows.Add(((double)xdataGridView[1,i].Value-(double)xdataGridView[1, 3].Value)/(int)xh,
                    (int)xdataGridView[2, i].Value,
                    ((double)xdataGridView[1, i].Value - (double)xdataGridView[1, 3].Value) / (int)xh * (int)xdataGridView[2, i].Value,
                    (((double)xdataGridView[1, i].Value - (double)xdataGridView[1, 3].Value) / (int)xh* ((double)xdataGridView[1, i].Value - (double)xdataGridView[1, 3].Value) / (int)xh) * (int)xdataGridView[2, i].Value,
                    ((double)ydataGridView[1, i].Value - (double)ydataGridView[1, 3].Value) / (int)yh,
                    (int)ydataGridView[2, i].Value,
                    ((double)ydataGridView[1, i].Value - (double)ydataGridView[1, 3].Value) / (int)yh * (int)ydataGridView[2, i].Value,
                    (((double)ydataGridView[1, i].Value - (double)ydataGridView[1, 3].Value) / (int)yh * ((double)ydataGridView[1, i].Value - (double)ydataGridView[1, 3].Value) / (int)yh) * (int)ydataGridView[2, i].Value);
            }

            //Подсчет сумм в столбцах
            int sum1 = 0, sum2 = 0, sum3 = 0, sum5 = 0, sum6 = 0, sum7 = 0;
            for (int i = 0; i < 7; i++)
            {
                sum1 = sum1 + (int)dataGridViewTask2[1, i].Value;
                sum2 = sum2 + Convert.ToInt32(dataGridViewTask2[2, i].Value);
                sum3 = sum3 + Convert.ToInt32(dataGridViewTask2[3, i].Value);
                sum5 = sum5 + (int)dataGridViewTask2[5, i].Value;
                sum6 = sum6 + Convert.ToInt32(dataGridViewTask2[6, i].Value);
                sum7 = sum7 + Convert.ToInt32(dataGridViewTask2[7, i].Value);
            }
            labelTask2.Text = "Σ";
            labelt21.Text = sum1.ToString();
            labelt22.Text = sum2.ToString();
            labelt23.Text = sum3.ToString();
            labelt24.Text = sum5.ToString();
            labelt25.Text = sum6.ToString();
            labelt26.Text = sum7.ToString();

            //Вычисление искомых оценок
            double u, u2, v, v2,su,sv;
            u = (double)sum2 / sum1;
            u2 = (double)sum3 / sum1;
            v = (double)sum6 / sum5;
            v2 = (double)sum7 / sum5;
            su = Math.Sqrt(50 / 49 * (u2 - u * u));
            sv = Math.Sqrt(50 / 49 * (v2 - v * v));
            xslash = (int)xh * u + (double)xdataGridView[1, 3].Value;
            yslash = (int)yh * v + (double)ydataGridView[1, 3].Value;
            xs = Math.Sqrt((int)xh * (int)xh * su * su);
            ys = Math.Sqrt((int)yh * (int)yh * sv * sv);

            labeltx1.Text = "X̄ = " + xslash.ToString();
            labeltx2.Text = "Sx = " + xs.ToString();
            labelty1.Text = "Ȳ = " + yslash.ToString();
            labelty2.Text = "Sy = " + ys.ToString();

            button3.Enabled = true;
        }

        //Проверить гипотезы о нормальном законе распределения
        private void button3_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
            tabControl1.SelectedTab = tabPage3;
            button3.Enabled = false;
        }
    }
}
