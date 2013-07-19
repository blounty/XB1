using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxOne.Core.Models;

namespace XboxOne.Core.WP8.Services
{
    public class GameService
    {
        public async Task<List<Game>> LoadGames()
        {
            var query = ParseObject.GetQuery("Game")
                .OrderByDescending(x => x["Name"]);
 
            IEnumerable<ParseObject> result = new List<ParseObject>();

            try
            {
                result = await query.FindAsync();
            }
            catch (Exception ex)
            {
                var errorItem = new ParseObject("Game");
                errorItem["Name"] = "error, please retry...";
                ((List<ParseObject>)result).Add(errorItem);
            }

            result = result.OrderByDescending(x => x["Name"]);

            var games = new List<Game>();
            foreach (var gameParseObject in result)
            {
                var game = await Game.CreateFromParseObject(gameParseObject);
                games.Add(game);
            }

            return games;
        }

        public async Task<Game> LoadGameById(string id)
        {
            var query = ParseObject.GetQuery("Game")
                .Where(g => g.ObjectId == id);

            IEnumerable<ParseObject> result = new List<ParseObject>();

            try
            {
                result = await query.FindAsync();
            }
            catch (Exception ex)
            {
                var errorItem = new ParseObject("Game");
                errorItem["Name"] = "error, please retry...";
                ((List<ParseObject>)result).Add(errorItem);
            }

            var game = await Game.CreateFromParseObject(result.FirstOrDefault());

            return game;
        }
    }
}
