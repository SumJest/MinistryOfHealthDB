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
    public partial class Form2 : Form
    {
        public User user;
        public Form2()
        {
            
            InitializeComponent();
            label1.Text = label1.Text.Replace("{NICK_NAME}", "Гость");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form3 form = new Form3();
            this.Hide();
            form.ShowDialog(this);
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            if (form.ShowDialog().Equals(DialogResult.OK))
            {
                user = form.user;
                label1.Text = label1.Text.Replace("Гость", form.textBox2.Text);
                button5.Enabled = false;
                
                if (user.Permissions.Contains("*.*") || user.Permissions.Contains("members.")) { button1.Enabled = true; }
                
                if (user.Permissions.Contains("*.*") || user.Permissions.Contains("rebuke.")) { button6.Enabled = true; }
            }
            else { return; }
            Console.WriteLine("Авторизовался пользователь: "  + user.Nickname + "\nС правами: " + user.Permissions);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form5 form = new Form5();
            form.ShowDialog(this);
            
        }
        
    }
}
