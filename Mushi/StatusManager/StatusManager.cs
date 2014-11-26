using Mushi.Helper;
using MushiDataTypes;
using System.Collections.Generic;
using System.Threading;

namespace Mushi.StatusManager
{
    public static class StatusManager
    {
        /// <summary>
        /// The Tasks
        /// </summary>
        private static readonly Dictionary<string, Timer> Tasks = new Dictionary<string, Timer>();

        /// <summary>
        /// The _lock
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Runs the offline timer.
        /// </summary>
        /// <param name="player">The player.</param>
        public static void RunOfflineTimer(Player player)
        {
            if (player == null) return;
            var task = GetTaskOnOffline(player);
            System.Runtime.InteropServices.GCHandle.Alloc(task);
            if (task == null) return;
            lock (_lock)
            {
                Timer timer;
                if (Tasks.TryGetValue(player.PlayerId, out timer))
                    Tasks[player.PlayerId] = task;
                else
                    Tasks.Add(player.PlayerId, task);
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public static void StopOfflineTimer(string id)
        {
            //TODO zalockovac se hned Task
            lock (_lock)
            {
                Timer timer;
                if (!Tasks.TryGetValue(id, out timer)) return;
                Tasks.Remove(id);
                timer.Dispose();
            }
        }

        /// <summary>
        /// Gets the task on offline.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        private static Timer GetTaskOnOffline(Player player)
        {
            return player == null 
                ? null 
                : new Timer(DoOnOffline, player, Properties.Settings.Default.OfflineTimeSpanMilliseconds, Timeout.Infinite);
        }

        /// <summary>
        /// Does the on offline.
        /// </summary>
        /// <param name="obj">The object.</param>
        private static void DoOnOffline(object obj)
        {
            var player = obj as Player;
            if (player == null) return;
            DisconnectHelper.DoOnOffline(player);
            StopOfflineTimer(player.PlayerId);
        }

    }
}
