using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreateSphinx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable table = new DataTable("DataTableLevel");
            table.Columns.Add("Номер уровня", typeof(int)); //0
            table.Columns.Add("Вопрос", typeof(string)); //1
            for (int i = 1; i <= 4; i++)
            {
                table.Columns.Add("Ответ " + i, typeof(string));
            }
            table.Columns.Add("Правильный ответ"); //6
            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                provDataGridProvNumberNotNull();
                var doc = new System.Xml.XmlDocument();
                doc.Load(open.FileName);
                DataTable table = dataGridView1.DataSource as DataTable;
                table.Clear();
                foreach (System.Xml.XmlNode node in doc.DocumentElement)
                {
                    try
                    {
                        if (node.Name == "lvl" && node.ChildNodes.Count == 4)
                        {
                            var row = table.NewRow();
                            row[0] = int.Parse(node.ChildNodes[0].InnerText);
                            row[1] = node.ChildNodes[1].InnerText;
                            string[] ans = node.ChildNodes[2].InnerText.Split(',');
                            row[2] = ans[0];
                            row[3] = ans[1];
                            row[4] = ans[2];
                            row[5] = ans[3];
                            row[6] = node.ChildNodes[3].InnerText;
                            table.Rows.InsertAt(row, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
                table = tableSort(table);
                table = notPerenos(table);
                dataGridView1.DataSource = table;
            }
        }

        private DataTable notPerenos(DataTable table)
        {
            int count_rows = table.Rows.Count,
                count_columns = table.Columns.Count;
            for (int i=0;i<count_rows;i++)
                for (int j=1;j<count_columns;j++)
                    table.Rows[i][j] = ((string)table.Rows[i][j]).Replace("\n"," ").Replace("\r"," ").Replace("  ", " ");
            return table;
        }

        private int provDataGridProvNumberNotNull()
        {
            int max_value = 1, count = dataGridView1.Rows.Count;
            bool notpovtor = true;
            while (notpovtor)
            {
                notpovtor = false;
                for (int i = 0; i < count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null
                        && dataGridView1.Rows[i].Cells[0].Value.ToString().Any())
                    {
                        max_value = (int)dataGridView1.Rows[i].Cells[0].Value;
                        for (int j = i + 1; j < count; j++)
                        {
                            if (dataGridView1.Rows[j].Cells[0].Value != null 
                                && dataGridView1.Rows[j].Cells[0].Value.ToString().Any())
                            {
                                int tmp = (int)dataGridView1.Rows[j].Cells[0].Value;
                                if (max_value == tmp)
                                {
                                    dataGridView1.Rows[j].Cells[0].Value = (tmp + 1);
                                    notpovtor = true;
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < count; i++)
                if (dataGridView1.Rows[i].Cells[0].Value != null
                    && dataGridView1.Rows[i].Cells[0].Value.ToString().Any())
                {
                    int value = (int)dataGridView1.Rows[i].Cells[0].Value;
                    if (max_value < value)
                        max_value = value + 1;
                }
            for (int i = 0; i < count; i++)
                if (dataGridView1.Rows[i].Cells[0].Value == null
                    || !dataGridView1.Rows[i].Cells[0].Value.ToString().Any())
                {
                    dataGridView1.Rows[i].Cells[0].Value = max_value;
                    max_value++;
                }
            max_value = 1;
            for (int i=0;i<count;i++)
            {
                int value = (int)dataGridView1.Rows[i].Cells[0].Value;
                if (value > max_value)
                    max_value = value;
            }
            return max_value + 1;
        }

        private DataTable tableSort(DataTable table)
        {
            int n = table.Rows.Count;
            for (int i=0;i<n;i++)
            {
                for (int j=i+1;j<n;j++)
                {
                    if ((int)table.Rows[i][0] > (int)table.Rows[j][0])
                    {
                        var tmp = table.Rows[i].ItemArray;
                        table.Rows[i].ItemArray = table.Rows[j].ItemArray;
                        table.Rows[j].ItemArray = tmp;
                    }
                }
            }
            return table;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.ColumnCount != 0 && dataGridView1.DataSource != null)
            {
                SaveFileDialog save = new SaveFileDialog();
                if (save.ShowDialog() == DialogResult.OK)
                {
                    List<string> lines = new List<string>();
                    lines.Add("<levels>");
                    DataTable table = (DataTable)dataGridView1.DataSource;
                    table = tableSort(table);
                    foreach (DataRow row in table.Rows)
                    {
                        lines.Add("  <lvl>");
                        lines.AddRange(getRowDataToXml(row));
                        lines.Add("  </lvl>");
                    }
                    lines.Add("</levels>");
                    System.IO.File.WriteAllLines(save.FileName, lines.ToArray());
                }
            }
        }

        private string[] getRowDataToXml(DataRow row)
        {
            string[] lines = new string[4];
            lines[0] = string.Format("       <N>{0}</N>", row[0].ToString());
            lines[1] = string.Format("       <Q>{0}</Q>", row[1].ToString());

            lines[2] = string.Format("       <A>{0},{1},{2},{3}</A>", new object[] { row[2].ToString(),
                row[3].ToString(), row[4].ToString(), row[5].ToString()});

            lines[3] = string.Format("       <T>{0}</T>", row[6].ToString());
            return lines;
        }

        /// <summary>
        /// Указывает на что редактируется ячейка
        /// </summary>
        private int changerowtable = -1;

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (changerowtable != -1) return;
                changerowtable = e.RowIndex;
                textBoxQuest.Text = (string)dataGridView1.Rows[changerowtable].Cells[1].Value;
                string[] ans = new string[]
                {
                    (string)dataGridView1.Rows[changerowtable].Cells[2].Value,
                    (string)dataGridView1.Rows[changerowtable].Cells[3].Value,
                    (string)dataGridView1.Rows[changerowtable].Cells[4].Value,
                    (string)dataGridView1.Rows[changerowtable].Cells[5].Value,
                };
                textBoxAnswer.Text = string.Join<string>(",", ans);
                textBoxTrueAnswer.Text = (string)dataGridView1.Rows[changerowtable].Cells[6].Value;
            }
        }

        private void button4AddRow_Click(object sender, EventArgs e)
        {
            if (textBoxQuest.TextLength > 0 && textBoxAnswer.TextLength > 0 && textBoxTrueAnswer.TextLength > 0)
            {
                int index = (changerowtable == -1) ? dataGridView1.NewRowIndex : changerowtable;
                if (index == -1)
                {
                    var table = ((DataTable)dataGridView1.DataSource);
                    index = table.Rows.Count;
                    table.Rows.Add(table.NewRow());
                }
                var row = dataGridView1.Rows[index].Cells;
                row[0].Value = provDataGridProvNumberNotNull() - 1;
                row[1].Value = textBoxQuest.Text;
                string[] mass = textBoxAnswer.Text.Split(',');
                row[2].Value = mass[0];
                row[3].Value = mass[1];
                row[4].Value = mass[2];
                row[5].Value = mass[3];
                row[6].Value = textBoxTrueAnswer.Text;
                button3CancelRow_Click(sender, e);
            }
        }

        private void button3CancelRow_Click(object sender, EventArgs e)
        {
            textBoxQuest.Clear();
            textBoxAnswer.Clear();
            textBoxTrueAnswer.Clear();
            changerowtable = -1;
        }

        bool change_true_answer = true;
        private void textBoxTrueAnswer_TextChanged(object sender, EventArgs e)
        {
            if (change_true_answer)
            {
                change_true_answer = false; bool find = false;
                string[] ansss = textBoxAnswer.Text.Split(',');
                for (int i = 0; i < ansss.Length; i++)
                {
                    if (ansss[i].ToLower() == textBoxTrueAnswer.Text.ToLower())
                    {
                        if (ansss[i] != textBoxTrueAnswer.Text)
                            textBoxTrueAnswer.Text = ansss[i];
                        find = true; break;
                    }
                }
                try
                {
                    int index = int.Parse(textBoxTrueAnswer.Text) - 1;
                    textBoxTrueAnswer.Text = ansss[index];
                    find = true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                if (find == false)
                    textBoxTrueAnswer.Clear();
                change_true_answer = true;
            }
        }

        bool change_all_answers = true;
        private void textBoxAnswer_TextChanged(object sender, EventArgs e)
        {
            if (change_all_answers)
            {
                change_true_answer = false;
                string[] ansss = textBoxAnswer.Text.Split(',');
                textBoxTrueAnswer.Enabled = (ansss.Length == 4 && ansss.All(x => x.Length > 0));
                change_true_answer = true;
            }
        }

        private void textBoxQuest_TextChanged(object sender, EventArgs e)
        {
            textBoxQuest.Text = textBoxQuest.Text.Replace("\n", " ").Replace("\n", " ").Replace("  ", " ");
            textBoxAnswer.Enabled = (textBoxQuest.TextLength > 0);
        }
    }
}
