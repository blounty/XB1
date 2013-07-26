using System;
namespace XboxOne.Core.Services
{
    public interface IVideoService
    {
        System.Threading.Tasks.Task<System.Collections.Generic.List<XboxOne.Core.Models.Video>> LoadVideosByGameId(string gameId);
    }
}
