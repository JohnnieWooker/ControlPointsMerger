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

namespace ControlPointsMerger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                foreach (string path in fileList)
                { 
                if (File.Exists(path))
                {
                    listBox1.Items.Add(path);
                }
                }
            }
            catch { }

        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            listBox1.Enabled = false;
            if (listBox1.Items.Count > 0)
            {
                int finalindex = 0;
                string mergedlines = "";
                string filesuffix = "final.csv";
                List<string> filepaths = new List<string>();
                List<int> pointindexes = new List<int>();
                string dir = GetMasterDir();
                foreach (string s in listBox1.Items) filepaths.Add(s);
                foreach (string s in filepaths)
                {
                    if (File.Exists(s))
                    {
                        int localpointindex = 0;
                        int newpointindex = 0;
                        StreamReader file = new StreamReader(s);
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            string searchbounded = "";
                            try
                            {
                                searchbounded = line.Substring(line.IndexOf(",") + 1);
                                searchbounded = searchbounded.Substring(0, searchbounded.IndexOf(","));
                                //MessageBox.Show(searchbounded);
                                if (searchbounded.IndexOf("point ") >= 0)
                                {
                                    searchbounded = searchbounded.Substring(searchbounded.IndexOf("point ") + 6);
                                    localpointindex = Convert.ToInt32(searchbounded);
                                    if (newpointindex != localpointindex) finalindex++;
                                    newpointindex = localpointindex;
                                    if (checkBox1.Checked)
                                    {
                                        try
                                        {
                                            string toreplace = line.Substring(0, line.LastIndexOf("\\"));
                                            string newreplace = textBox1.Text;
                                            newreplace=newreplace.Replace("/", "\\");
                                            //MessageBox.Show(newreplace[newreplace.Length - 1].ToString());
                                            if (newreplace.LastIndexOf("\\")==newreplace.Length-1) newreplace = newreplace.Substring(0, newreplace.Length - 1);
                                            line = line.Replace(toreplace, newreplace);
                                        }
                                        catch { }
                                            }
                                    mergedlines += line.Replace("point " + localpointindex.ToString(), "point " + finalindex) + "\r\n";
                                   
                                }

                            }
                            catch { }

                        }
                        finalindex++;
                        // MessageBox.Show(newpointindex.ToString());

                    }


                }

                string finalfile = dir + filesuffix;
                if (File.Exists(finalfile)) File.Delete(finalfile);
                File.WriteAllText(finalfile, mergedlines);
            }
            button1.Enabled = true;
            listBox1.Enabled = true;
        }

        string GetMasterDir()
        {
            string dir = "";
            if (listBox1.Items.Count>0)
            {
                dir = listBox1.Items[0].ToString();
                dir=dir.Substring(0, dir.LastIndexOf("\\")+1);
            }
            return dir;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { panel1.Visible = true; panel1.Size = new Size(237, 28);button1.Location = new Point(button1.Location.X, button1.Location.Y + 30); this.Size = new Size(this.Size.Width, this.Size.Height + 30); }
            else { panel1.Visible = false; panel1.Size = new Size(237, 0); button1.Location = new Point(button1.Location.X, button1.Location.Y - 30); this.Size = new Size(this.Size.Width, this.Size.Height - 30); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Visible = false; panel1.Size = new Size(237, 0); button1.Location = new Point(button1.Location.X, button1.Location.Y - 30); this.Size = new Size(this.Size.Width, this.Size.Height - 30);
        }
    }
}
