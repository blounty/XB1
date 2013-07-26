using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxOne.Core.Models
{
    public class Video
        : BaseModel
    {
        public string GameId { get; set; }

        public string Summary { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Title { get; set; }

        public string VideoId { get; set; }

        public static async Task<Video> CreateFromParseObject(ParseObject parseObject)
        {
            return await Task.Run<Video>(() =>
            {
                var video = new Video();

                video.Id = (string)parseObject.ObjectId;

                if (parseObject.ContainsKey("Summary"))
                {
                    video.Summary = (string)parseObject["Summary"];
                }

                if (parseObject.ContainsKey("ThumbnailUrl"))
                {
                    video.ThumbnailUrl = (string)parseObject["ThumbnailUrl"];
                }

                if (parseObject.ContainsKey("Title"))
                {
                    video.Title = (string)parseObject["Title"];
                }

                if (parseObject.ContainsKey("VideoId"))
                {
                    video.VideoId = (string)parseObject["VideoId"];
                }

                return video;
            });
        }
    }
}
