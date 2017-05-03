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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Rebukes.AddNewRebuke(Rebukes.GetService(Rebukes.GetSheetCredentials()), Rebukes.SpreadsheetId, new Rebuke(textBox1.Text, textBox2.Text, dateTimePicker1.Value, textBox3.Text, (this.Owner as Form2).user.Nickname, textBox4.Text));
                Console.WriteLine("Успешно отправлено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
    }
}
