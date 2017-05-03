using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinistryOfHealthDB
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = this.Owner as Form3;
            string perms = "";
            foreach (string line in textBox4.Lines)
            {
                perms += perms=="" ? line : ";" +  line;
            }
            form3.added.Add(new User(form3.listBox1.Items.Count, textBox1.Text, textBox2.Text, perms, int.Parse(textBox3.Text)));
            form3.listBox1.Items.Add(form3.listBox1.Items.Count + " " + textBox1.Text);
            this.Close();
        }
    }
}
