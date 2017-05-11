using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace MinistryOfHealthDB
{
    public class Imgur
    {
        public string AppId { get; set; }
        public string BaseUrl = "https://api.imgur.com/3/";

        public Imgur(string appid)
        {
            AppId = appid;
        }

        public async Task<ImgurCreateAlbum> CreateAlbumAnonymous(IEnumerable<string> imageIds, string title, string description, ImgurAlbumPrivacy privacy, ImgurAlbumLayout layout, string coverImageId)
        {

            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                var formContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("deletehashes", imageIds.Aggregate((a,b) => a + "," + b)),
                 });

                HttpResponseMessage response = await client.PostAsync(new Uri(BaseUrl + "album"), formContent);
                
                string content = await response.Content.ReadAsStringAsync();

                ImgurRootObject<ImgurCreateAlbum> createRoot = JsonConvert.DeserializeObject<ImgurRootObject<ImgurCreateAlbum>>(content);

                return createRoot.Data;
            }
        }
        void SetHeaders(HttpClient client)
        {
            if (string.IsNullOrWhiteSpace(AppId))
                throw new Exception("AppId is not set, please specify");

            client.DefaultRequestHeaders.Add("Authorization", "Client-ID " + AppId);
        }
        string GetNameFromEnum<T>(int selected) where T : struct
        {
            string value = Enum.GetName(typeof(T), selected).ToLower();

            if (value == "none")
                value = "";

            return value;
        }
    }
    public class ImgurCreateAlbum
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("deletehash")]
        public string DeleteHash { get; set; }
    }
    public class ImgurRootObject<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
    }
    public enum ImgurAlbumPrivacy
    {
        Public,
        Hidden,
        Secret,
        None
    }
    public enum ImgurAlbumLayout
    {
        Blog,
        Grid,
        Horizontal,
        Vertical,
        None
    }
}
