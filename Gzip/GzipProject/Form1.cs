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

namespace GzipProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Компрессия или Деко*?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            compressFile(r == DialogResult.Yes);
        }

        private void compressFile(bool compress_bool)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            if (open.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog save = new SaveFileDialog();
                if (save.ShowDialog() == DialogResult.OK)
                {
                    string ext = compress_bool ? "gz" : "xml";
                    if (compress_bool)
                    {
                        FileStream fs_write = new FileStream(Path.ChangeExtension(save.FileName, ext), FileMode.Create, FileAccess.Write);
                        System.IO.Compression.GZipStream g = new System.IO.Compression.GZipStream(fs_write, System.IO.Compression.CompressionLevel.Optimal, false);
                        byte[] mass = File.ReadAllBytes(open.FileName);
                        g.Write(mass, 0, mass.Length);
                        g.Close();
                    }
                    else
                    {
                        FileStream fs_open = new FileStream(open.FileName, FileMode.Open, FileAccess.Read);
                        System.IO.Compression.GZipStream g = new System.IO.Compression.GZipStream(fs_open, System.IO.Compression.CompressionMode.Decompress, false);
                        byte[] mass = new byte[fs_open.Length];
                        int h; List<byte> list = new List<byte>();
                        while ((h = g.Read(mass, 0, mass.Length)) > 0)
                        {
                            list.AddRange(mass.Take(h));
                        }
                        g.Close();
                        File.WriteAllBytes(Path.ChangeExtension(save.FileName, ext), list.ToArray());
                    }
                }
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Компрессия или Деко*?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            compressFile(r == DialogResult.Yes);
        }
    }
}
