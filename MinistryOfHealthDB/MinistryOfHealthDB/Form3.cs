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
    public partial class Form3 : Form
    {
        List<User> users;
        public List<User> added = new List<User>();

        public Form3()
        {
            InitializeComponent();
            users = Spreadsheets.GetUsers();
            foreach ( User user in users)
            {
                listBox1.Items.Add(user.Index + " " + user.Nickname);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            foreach (User user in added)
            {
                Spreadsheets.AddNewUser(Spreadsheets.GetService(Spreadsheets.GetSheetCredentials()), Spreadsheets.SpreadsheetId, user);
            }
            label3.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
