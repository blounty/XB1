using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxOne.Core.Models;

namespace XboxOne.Core.Services
{
    public class VideoService : XboxOne.Core.Services.IVideoService
    {
        public async Task<List<Video>> LoadVideosByGameId(string gameId)
        {

            var query = ParseObject.GetQuery("Video").Where(v => v.Get<string>("GameId") == gameId);

            IEnumerable<ParseObject> result = new List<ParseObject>();

            try
            {
                result = await query.FindAsync();
            }
            catch (Exception ex)
            {
                var errorItem = new ParseObject("Video");
                errorItem["Summary"] = "error, please retry...";
                ((List<ParseObject>)result).Add(errorItem);
            }

            result = result.OrderByDescending(x => x.CreatedAt);

            var videos = new List<Video>();
            foreach (var videoParseObject in result)
            {
                var video = await Video.CreateFromParseObject(videoParseObject);
                videos.Add(video);
            }

            return videos;
        }
    }
}
