using System;
namespace XboxOne.Core.Services
{
    public interface IGameService
    {
        System.Threading.Tasks.Task<XboxOne.Core.Models.Game> LoadGameById(string id);
        System.Threading.Tasks.Task<System.Collections.Generic.List<XboxOne.Core.Models.Game>> LoadGames();
    }
}
