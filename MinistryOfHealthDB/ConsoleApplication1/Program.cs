
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Net;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        public static string ClientSecret = "client_secret.json";
        public static readonly string[] ScopesSheets = { SheetsService.Scope.Spreadsheets };
        public static readonly string AppName = "GoogleSheetsStart";
        public static readonly string SpreadsheetId = "1SFfSVXwQU2Rn2X2zHHeh8A1dG5izspYwuvYNMH85wvk";
        public static char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public static string[,] Data = new string[,]
            {
                { "A1", "B1", "C1"},
                { "A2", "B2", "C2"},
                { "A3", "B3", "C3" }
            };

        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            using (var w = new WebClient())
            {
                var values = new NameValueCollection
                {
                {"image", Convert.ToBase64String(File.ReadAllBytes(path))}
                };

                w.Headers.Add("Authorization", "Client-ID 56e73ed9f6e02c9");
                byte[] response = w.UploadValues("https://api.imgur.com/3/upload.xml", values);
                Console.WriteLine("URL: http://imgur.com/" + Regex.Match(XDocument.Load(new MemoryStream(response)).ToString(), @"(?<=<id>)(.*)(?=</id>)"));
            }
            Console.ReadKey();

                
            }
        }
    }

