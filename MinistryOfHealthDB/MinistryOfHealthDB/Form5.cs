using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

                Rebukes.AddNewRebuke(Rebukes.GetService(Rebukes.GetSheetCredentials()), Rebukes.SpreadsheetId, new Rebuke(textBox1.Text, textBox2.Text, dateTimePicker1.Value, textBox3.Text, (this.Owner as Form2).user.Nickname, textBox4.Text.StartsWith("http://imgur.com/") ? textBox4.Text : getImgurUrl(textBox4.Text)));
                Console.WriteLine("Успешно отправлено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

        private void textBox4_DragDrop(object sender, DragEventArgs e)
        {
            string[] filedrop = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filedrop.Length > 1)
            {
                MessageBox.Show("Пожалуйста выберете только один файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!File.Exists(filedrop[0]))
            {
                MessageBox.Show("Пожалуйста выберете файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            textBox4.Text = filedrop[0];
        }

        private void textBox4_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        private string getImgurUrl(string validpath)
        {
            try
            {
                using (var w = new WebClient())
                {
                    var values = new NameValueCollection
                {
                    {"image", Convert.ToBase64String(File.ReadAllBytes(validpath))}
                };

                    w.Headers.Add("Authorization", "Client-ID 56e73ed9f6e02c9");
                    byte[] response = w.UploadValues("https://api.imgur.com/3/upload.xml", values);
                    return "http://imgur.com/" + System.Text.RegularExpressions.Regex.Match(System.Xml.Linq.XDocument.Load(new MemoryStream(response)).ToString(), @"(?<=<id>)(.*)(?=</id>)");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); return null; }
            }
        }
    }

