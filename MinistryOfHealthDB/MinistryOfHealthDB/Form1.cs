using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;

namespace MinistryOfHealthDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public User user;
        private void button1_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            List<User> users = Spreadsheets.GetUsers();
            List<string> nodes = new List<string>();
            foreach (User user in users)
            {
                nodes.Add(user.Nickname + ":" + user.Password);
            }
            label3.Visible = false;
            if (!nodes.Contains(textBox2.Text + ":" + textBox1.Text))
            {
                MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            user = users[nodes.IndexOf(textBox2.Text + ":" + textBox1.Text)];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
