using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using XboxOne.Core.Services;
using Microsoft.Phone.Shell;
using System.Linq;
using Parse;
using System;
using System.Collections.Generic;

namespace XboxOne.PeriodicTask
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override async void OnInvoke(ScheduledTask task)
        {
            try
            {
                var newsService = new NewsService();

                List<ShellTile> appTiles = ShellTile.ActiveTiles.ToList();
                    
                 var appTile = appTiles.Where(tile => tile.NavigationUri.ToString() == "/").FirstOrDefault();

                if (appTile != null)
                {
                    ParseClient.Initialize("5yU6Uf5QqT066zOB52KBZBAhf9qnrRPVlZrRzvp7", "pjPDnc7GZKERP3yiAw9sP3lXy7RRXwEaG4CGN8Qp");

                   

                    var newsItems = await newsService.LoadNews(1, 0);

                    var tileData = new FlipTileData()
                    {
                        BackContent = newsItems[0].Summary.Length > 25 ? newsItems[0].Summary.Substring(0,24) + "..." : newsItems[0].Summary,
                        BackTitle = newsItems[0].Title,
                        WideBackContent = newsItems[0].Summary,
                        BackgroundImage = new System.Uri(newsItems[0].ImageUrl),
                        WideBackgroundImage = new System.Uri(newsItems[0].ImageUrl),
                        Title = "XB1"
                    };

                    appTile.Update(tileData);
                }

                var secondaryTiles = ShellTile.ActiveTiles.Where(tile => tile.NavigationUri.ToString().Contains("&_id="));
                var gameService = new GameService();
                foreach (var tile in secondaryTiles)
                {
                    var gameId = tile.NavigationUri.ToString().Split(new []{"&_id="}, StringSplitOptions.RemoveEmptyEntries)[1];
                    var newsItems = newsService.LoadNewsByGameId(gameId).Result;
                    var game = gameService.LoadGameById(gameId).Result;
                    if (newsItems.Count > 0)
                    {
                        var tileData = new FlipTileData()
                        {
                            BackContent = newsItems[0].Summary.Length > 25 ? newsItems[0].Summary.Substring(0,24) + "..." : newsItems[0].Summary,
                            BackTitle = newsItems[0].Title,
                            WideBackContent = newsItems[0].Summary,
                            BackgroundImage = new System.Uri(newsItems[0].ImageUrl),
                            WideBackgroundImage = new System.Uri(newsItems[0].ImageUrl),
                            Title = string.Format("XB1 - {0}", game.Name)
                        };
                        tile.Update(tileData);
                    }
                    
                }

            }
            finally
            {
                NotifyComplete();
            }
        }
    }
}