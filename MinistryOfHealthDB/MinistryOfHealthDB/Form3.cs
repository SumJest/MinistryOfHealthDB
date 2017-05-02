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
    }
}
