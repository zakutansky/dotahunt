using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Mushi.Context;
using MushiDataTypes.Enums;
using Mushi.Properties;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mushi.Hubs
{
    public class MushiHub : Hub
    {
        /// <summary>
        /// The ack checkers
        /// </summary>
        private static readonly ConcurrentDictionary<string, AutoResetEvent> AckWaiters = new ConcurrentDictionary<string, AutoResetEvent>();

        #region Acknowlagement

        /// <summary>
        /// Executes the with ack.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="receiverId">The receiver identifier.</param>
        /// <param name="parameter"></param>
        /// <param name="key">The key.</param>
        public static void ExecuteWithAck(Action<string, string, string> function, string receiverId, string parameter, string key)
        {
            var waiter = new AutoResetEvent(false);
            AckWaiters.GetOrAdd(key, waiter);
            var numberOfWaits = Settings.Default.OfflineTimeSpanMilliseconds /
                                Settings.Default.AckWaitingTimeSpanMilliseconds;
            for (var i = 0; i <= numberOfWaits; i++)
            {
                function(receiverId, parameter, key);
                var result = waiter.WaitOne(Settings.Default.AckWaitingTimeSpanMilliseconds);
                if (result || i == numberOfWaits)
                {
                    if (AckWaiters.TryRemove(key, out waiter))
                    {
                        waiter.Dispose();
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Acks the specified key. Called from client.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Ack(string key)
        {
            AutoResetEvent waiter;
            var val = AckWaiters.TryGetValue(key, out waiter);
            if (val)
            {
                waiter.Set();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Called when the connection connects to this hub instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" />
        /// </returns>
        public override Task OnConnected()
        {
            var pc = new ProgamerContext();
            var player = pc.GetProgamer(Context.User.Identity.GetUserId());
            if (player != null && player.PlayerType == PlayerTypesEnum.Pro)
            {
                StatusManager.StatusManager.StopOfflineTimer(player.PlayerId);
            }
            return base.OnConnected();
        }

        /// <summary>
        /// Called when [disconnected].
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var pc = new ProgamerContext();
            var player = pc.GetProgamer(Context.User.Identity.GetUserId());
            if (player != null)
            {
                StatusManager.StatusManager.RunOfflineTimer(player);
            }
            return base.OnDisconnected(stopCalled);
        }
        #endregion
    }
}