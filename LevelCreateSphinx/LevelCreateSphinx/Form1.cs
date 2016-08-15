using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                var doc = new System.Xml.XmlDocument();
                doc.Load(open.FileName);
                DataTable table = new DataTable("DataTableLevel");
                table.Columns.Add("Номер уровня",typeof(int));
                table.Columns.Add("Вопрос", typeof(string));
                for (int i = 1; i <= 4; i++)
                {
                    table.Columns.Add("Ответ " + i, typeof(string));
                }
                table.Columns.Add("Правильный ответ");
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
                            table.Rows.InsertAt(row,0);
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
                dataGridView1.DataSource = table;
            }
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
            lines[0] = string.Format("       <N>{0}</N>",row[0].ToString());
            lines[1] = string.Format("       <Q>{0}</Q>", row[1].ToString());

            lines[2] = string.Format("       <A>{0},{1},{2},{3}</A>", new object[] { row[2].ToString(),
                row[3].ToString(), row[4].ToString(), row[5].ToString()});

            lines[3] = string.Format("       <T>{0}</T>", row[6].ToString());
            return lines;
        }
    }
}
