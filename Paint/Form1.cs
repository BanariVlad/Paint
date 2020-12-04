using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        private Draw Draw = new Draw();
        public Form1()
        {
            Draw.GenerateMenu(this);
            InitializeComponent();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) => Draw.SaveImage();
        private void openToolStripMenuItem_Click(object sender, EventArgs e) => Draw.OpenImage();

        private void textBox1_TextChanged(object sender, EventArgs e) => Draw.SetWeight(Convert.ToInt32(textBox1.Text));
    }
}