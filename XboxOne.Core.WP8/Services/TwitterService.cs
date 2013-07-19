using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XboxOne.Core.Models;

namespace XboxOne.Core.WP8.Services
{
    public class TwitterService
    {
        const string TWEET_AUTH_URL = "https://api.twitter.com/oauth2/token";

        private string twitterAccessToken = "";

        private long maxId = default(long);

        public string SearchTerm { get; set; }

        public int ReturnCount { get; set; }

        public TwitterService(string searchTerm, int returnCount)
        {
            this.SearchTerm = searchTerm;
            this.ReturnCount = returnCount;
        }

        public void ResetFilters()
        {
            this.maxId = default(long);
        }

        public async Task<bool> AuthorizeTwitter()
        {
            if (!string.IsNullOrEmpty(this.twitterAccessToken))
                return true;

            try
            {
                var authTokens = "QUZksaqAdqZFxefaQ5cPQ:9vNnFn1SrtUAoWa1eNXjTFNpySQN2b5icTs10uDE4g";

                var base64AuthTokens = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(authTokens));

                var twitterAuthClient = new HttpClient();

                twitterAuthClient.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", base64AuthTokens));
                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await twitterAuthClient.PostAsync(TWEET_AUTH_URL, content);

                var responseJson = JValue.Parse(await response.Content.ReadAsStringAsync());

                this.twitterAccessToken = responseJson["access_token"].ToString();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<List<TwitterItem>> LoadTweets()
        {
            var twitterItems = new List<TwitterItem>();

            if (string.IsNullOrEmpty(this.twitterAccessToken))
            {
                return twitterItems;
            }

            var tweetClient = new HttpClient();

            tweetClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.twitterAccessToken);
            try
            {
                var response = await tweetClient.GetAsync(this.GenerateTwitterUrl());

                var jsonString = await response.Content.ReadAsStringAsync();
                var resultsJson = JObject.Parse(jsonString);



                var tweetArrayJson = (JArray)resultsJson["statuses"];

                foreach (JObject tweetJson in tweetArrayJson)
                {
                    var twitterItem = new TwitterItem
                    {
                        Author = tweetJson["user"]["screen_name"].Value<string>(),
                        AvatarUrl = tweetJson["user"]["profile_image_url"].Value<string>(),
                        Tweet = tweetJson["text"].Value<string>(),
                        Id = tweetJson["id"].Value<string>()
                    };
                    var createdDateString = tweetJson["created_at"].Value<string>();

                    twitterItem.PublishDate = createdDateString.Remove(createdDateString.Length - 11);
                    twitterItems.Add(twitterItem);
                }
            }
            catch (Exception)
            {
                var twitterItem = new TwitterItem
                {
                    Author = "",
                    AvatarUrl = "",
                    Tweet = "error, please retry...",
                    Id = ""
                };

                twitterItems.Add(twitterItem);

                return twitterItems;
            }

            this.maxId = Convert.ToInt64(twitterItems.Last().Id) - 1;

            return twitterItems;
        }

        private string GenerateTwitterUrl()
        {
            var url = string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}&count={1}", this.SearchTerm, this.ReturnCount);

            if (this.maxId != default(long))
            {
                url = string.Format("{0}&max_id={1}", url, this.maxId);
            }

            return url;
        }
    }
}
