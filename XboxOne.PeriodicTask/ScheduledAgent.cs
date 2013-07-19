using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using XboxOne.Core.WP8.Services;
using Microsoft.Phone.Shell;
using System.Linq;
using Parse;

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
                ParseClient.Initialize("5yU6Uf5QqT066zOB52KBZBAhf9qnrRPVlZrRzvp7", "pjPDnc7GZKERP3yiAw9sP3lXy7RRXwEaG4CGN8Qp");

                var newsService = new NewsService();
                var newsItems = await newsService.LoadNews(1, 0);

                ShellTile appTile = ShellTile.ActiveTiles.First();

                var tileData = new FlipTileData()
                {
                    BackContent = newsItems[0].Summary,
                    BackTitle = newsItems[0].Title,
                    WideBackContent = newsItems[0].Summary,
                    BackgroundImage = new System.Uri(newsItems[0].ImageUrl),
                    WideBackgroundImage =  new System.Uri(newsItems[0].ImageUrl),
                    Title = "XB1"
                };

                appTile.Update(tileData);
            }
            finally
            {
                NotifyComplete();
            }
        }
    }
}