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

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text.Contains(","))
                {
                    string text = "";
                    string[] paths = textBox4.Text.Split(',');
                    for(int i = 0; i < paths.Length; i++)
                    {
                        string path = paths[i];
                        string deletehash = getImgurDeleteHash(path);
                        text += text == "" ? deletehash : "," + deletehash;
                        Console.WriteLine(i+1 + "/" + paths.Length);
                    }
                    string[] deletehashes = text.Split(',');
                    Rebukes.AddNewRebuke(Rebukes.GetService(Rebukes.GetSheetCredentials()), Rebukes.SpreadsheetId, new Rebuke(textBox1.Text, textBox2.Text, dateTimePicker1.Value, textBox3.Text, (this.Owner as Form2).user.Nickname, "http://imgur.com/a/" + await getIdAlbum(deletehashes)));
                    Console.WriteLine("Успешно отправлено.");
                }
                else
                {
                    Rebukes.AddNewRebuke(Rebukes.GetService(Rebukes.GetSheetCredentials()), Rebukes.SpreadsheetId, new Rebuke(textBox1.Text, textBox2.Text, dateTimePicker1.Value, textBox3.Text, (this.Owner as Form2).user.Nickname, !File.Exists(textBox4.Text) ? textBox4.Text : getImgurUrl(textBox4.Text)));
                    Console.WriteLine("Успешно отправлено.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private async Task<string> getIdAlbum(string[] deletehashes)
        {
            Imgur imgur = new Imgur("56e73ed9f6e02c9");
            ImgurCreateAlbum ica = await imgur.CreateAlbumAnonymous(deletehashes, "", "", ImgurAlbumPrivacy.Public, ImgurAlbumLayout.Horizontal, "");
            return ica.Id;
        }
        private void textBox4_DragDrop(object sender, DragEventArgs e)
        {
            string[] filedrop = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filedrop.Length > 1)
            {
                string text = "";
                foreach (string a in filedrop)
                {
                    if (text.Contains(a)) { continue; }
                    text += text == "" ? a : "," + a;
                }
                textBox4.Text = text;
                return;
            }else
            {
                textBox4.Text = filedrop[0];
            }
            if (!File.Exists(filedrop[0]))
            {
                MessageBox.Show("Пожалуйста выберете файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
           
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
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); return validpath; }
           

        }
        private string getImgurDeleteHash(string validpath)
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


                    return System.Text.RegularExpressions.Regex.Match(System.Xml.Linq.XDocument.Load(new MemoryStream(response)).ToString(), @"(?<=<deletehash>)(.*)(?=</deletehash>)").ToString();

                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); return validpath; }


        }

    }
    }

