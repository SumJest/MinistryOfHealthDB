using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;

namespace MinistryOfHealthDB
{
    class Rebukes
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

        public static List<User> GetUsers()
        {

            UserCredential uc = GetSheetCredentials();
            SheetsService ss = GetService(uc);

            int height = default(int);
            List<string[]> list = new List<string[]>();
            for (int i = 0; i < 5; i++)
            {
                string[] s = GetFirstCell(ss, String.Format("'Rebuke'!{0}1:{0}", alphabet[i]), SpreadsheetId).Split('|');
                height = s.Length;
                list.Add(s);
            }
            List<User> users = new List<User>();
            for (int i = 0; i < height; i++)
            {
                User user;
                string message = "";
                foreach (string[] s in list)
                {
                    message += message == "" ? s[i] : "|" + s[i];
                }
                string[] data = message.Split('|');
                user = new User(int.Parse(data[0]), data[1], data[2], data[3], int.Parse(data[4]));
                users.Add(user);
            }
            return users;
            //foreach (User user in users)
            //{
            //    Console.WriteLine(user.Index + " Nick: " + user.Nickname + " Password: " + user.Password + " Perms: " + user.Permissions + " Rank:" + user.Rank);
            //}
            //Console.ReadKey();
        }

        public static UserCredential GetSheetCredentials()
        {
            using (FileStream stream = new FileStream(ClientSecret, FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.Combine(Directory.GetCurrentDirectory(), "sheetsCreds.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, ScopesSheets, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }
        }
        public static SheetsService GetService(UserCredential credential)
        {
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName

            });
        }
        public static void FillSpreadsheet(SheetsService service, string spreadsheetId, string[,] data)
        {
            List<Request> requests = new List<Request>();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                List<CellData> values = new List<CellData>();
                for (int j = 0; j < data.GetLength(i); j++)
                {
                    values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = data[i, j] } });

                }
                requests.Add(new Request
                {
                    UpdateCells = new UpdateCellsRequest
                    {
                        Start = new GridCoordinate
                        {
                            SheetId = 0,
                            RowIndex = i,
                            ColumnIndex = 0
                        },
                        Rows = new List<RowData> { new RowData { Values = values } },
                        Fields = "userEnteredValue"
                    }

                }
                );
            }
            BatchUpdateSpreadsheetRequest busr = new BatchUpdateSpreadsheetRequest
            {
                Requests = requests
            };
            service.Spreadsheets.BatchUpdate(busr, spreadsheetId).Execute();

        }
        public static void AddNewRebuke(SheetsService service, string spreadsheetId, Rebuke user)
        {
            Console.WriteLine(GetNistTime().ToString());
            List<Request> requests = new List<Request>();
            List<CellData> values = new List<CellData>();
            values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = TimeZoneInfo.ConvertTime(GetNistTime(), TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")).ToString() } });
            values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = user.EmployeeNickname.ToString() } });
            values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = user.Reason.ToString() } });
            values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = user.Date.Date.ToShortDateString() } });
            values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = user.Date.AddDays(Double.Parse(user.Term)).Date.ToShortDateString() } });
            values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = user.NickName.ToString() } });
            values.Add(new CellData { UserEnteredValue = new ExtendedValue { StringValue = user.Screenshot.ToString() } });

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, "'Rebuke'!A1:A");
            int index = request.Execute().Values.Count;
            requests.Add(new Request { UpdateCells = new UpdateCellsRequest { Start = new GridCoordinate { SheetId = 204848745, RowIndex = index, ColumnIndex = 0 }, Rows = new List<RowData> { new RowData { Values = values } }, Fields = "userEnteredValue" } });
            BatchUpdateSpreadsheetRequest busr = new BatchUpdateSpreadsheetRequest
            {
                Requests = requests
            };
            service.Spreadsheets.BatchUpdate(busr, spreadsheetId).Execute();
        }
        public static DateTime GetNistTime()
        {
            DateTime dateTime = DateTime.MinValue;
            /**/
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore); //No caching
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader stream = new StreamReader(response.GetResponseStream());
                string html = stream.ReadToEnd();//<timestamp time=\"1395772696469995\" delay=\"1395772696469995\"/>
                string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                double milliseconds = Convert.ToInt64(time) / 1000.0;
                dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
            }

            return dateTime;
        }
        public static string GetFirstCell(SheetsService service, string range, string spreadsheetId)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();

            string result = null;
            try
            {
                foreach (var value in response.Values)
                {
                    result += result == null ? value[0] : "|" + value[0];
                }
            }
            catch (Exception e) { result = ""; Console.WriteLine(e.Message); }

            return result;

        }


    }
    public class Rebuke
    {
        public string EmployeeNickname { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public string Term { get; set; }
        public string NickName { get; set; }
        public string Screenshot { get; set; }

        public Rebuke(string employeenickname, string reason, DateTime date, string term, string nickname, string screenshot)
        {
            EmployeeNickname = employeenickname;
            Reason = reason;
            Date = date;
            Term = term;
            NickName = nickname;
            Screenshot = screenshot;
        }

        public Rebuke()
        {

        }
    }
}
