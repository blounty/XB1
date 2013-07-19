using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxOne.Core.Models
{
    public class Game
        : BaseModel
    {
        public string Developer { get; set; }

        public string ImageUrl { get; set; }

        public string Publisher { get; set; }

        public string Detail { get; set; }

        public string PublisherDeveloper { get { return (!string.IsNullOrEmpty(this.Publisher) && !string.IsNullOrEmpty(this.Developer)) ? string.Format("{0}/{1}", this.Publisher, this.Developer) : (string.IsNullOrEmpty(this.Publisher) ? this.Developer : this.Publisher); } }

        public string Name { get; set; }

        public static async Task<Game> CreateFromParseObject(ParseObject parseObject)
        {
            return await Task.Run<Game>(() =>
                {
                    var game = new Game();

                    game.Id = (string)parseObject.ObjectId;

                    if (parseObject.ContainsKey("Developer"))
                    {
                        game.Developer = (string)parseObject["Developer"];
                    }

                    if (parseObject.ContainsKey("Publisher"))
                    {
                        game.Publisher = (string)parseObject["Publisher"];
                    }

                    if (parseObject.ContainsKey("Name"))
                    {
                        game.Name = (string)parseObject["Name"];
                    }

                    if (parseObject.ContainsKey("Detail"))
                    {
                        game.Detail = (string)parseObject["Detail"];
                    }

                    if (parseObject.ContainsKey("ImageUrl"))
                    {
                        game.ImageUrl = (string)parseObject["ImageUrl"];
                    }

                    return game;
                });
        }
    }
}
