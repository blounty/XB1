using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XboxOne.Core.ViewModels
{
    public class YouTubeContentViewModel
        :MvxViewModel
    {
        private string videoId;

        public string VideoId
        {
            get { return videoId; }
            set { videoId = value; }
        }

        private string videoUrl;

        public string VideoUrl
        {
            get { return videoUrl; }
            set 
            { 
                videoUrl = value;
                this.RaisePropertyChanged(() => this.VideoUrl);
            }
        }

        public async void Init(string videoId)
        {
            this.VideoId = videoId;

            await this.GetVideoUri(this.VideoId);
        }

        private  async Task GetVideoUri(string youTubeId)
        {
            var youTubeClient = new HttpClient();
            youTubeClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");

            var result = await youTubeClient.GetAsync("https://www.youtube.com/watch?v=" + youTubeId + "&nomobile=1");

            var urls = new List<YouTubeUri>();
            try
            {
                var match = Regex.Match(await result.Content.ReadAsStringAsync(), "url_encoded_fmt_stream_map\": \"(.*?)\"");
                var data = Uri.UnescapeDataString(match.Groups[1].Value);

                var arr = Regex.Split(data, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split by comma but outside quotes
                foreach (var d in arr)
                {
                    var url = "";
                    var signature = "";
                    var tuple = new YouTubeUri();
                    foreach (var p in d.Replace("\\u0026", "\t").Split('\t'))
                    {
                        var index = p.IndexOf('=');
                        if (index != -1 && index < p.Length)
                        {
                            try
                            {
                                var key = p.Substring(0, index);
                                var value = Uri.UnescapeDataString(p.Substring(index + 1));
                                if (key == "url")
                                    url = value;
                                else if (key == "itag")
                                    tuple.Itag = int.Parse(value);
                                else if (key == "type" && value.Contains("video/mp4"))
                                    tuple.Type = value;
                                else if (key == "sig")
                                    signature = value;
                            }
                            catch { }
                        }
                    }

                    tuple.url = url + "&signature=" + signature;
                    if (tuple.IsValid)
                        urls.Add(tuple);
                }

                var minTag = 18;
                var maxTag = 18;
                foreach (var u in urls.Where(u => u.Itag < minTag || u.Itag > maxTag).ToArray())
                    urls.Remove(u);
            }
            catch (Exception ex)
            {
                this.Close(this);
                return;
            }

            var entry = urls.OrderByDescending(u => u.Itag).FirstOrDefault();
            if (entry != null)
            {
                this.VideoUrl = entry.Uri.ToString();
            }
            else
            {
                this.Close(this);
            }

        }
    }

    public class YouTubeUri
    {
        internal string url;

        public Uri Uri { get { return new Uri(url, UriKind.Absolute); } }
        public int Itag { get; set; }
        public string Type { get; set; }

        public bool IsValid
        {
            get { return url != null && Itag > 0 && Type != null; }
        }
    }
}
