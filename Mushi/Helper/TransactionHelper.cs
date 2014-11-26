using Microsoft.AspNet.Identity;
using Mushi.Context;
using MushiDataTypes;
using MushiDataTypes.Enums;
using Newtonsoft.Json;
using System.Web;

namespace Mushi.Helper
{
    public class TransactionHelper
    {
        #region Properties
        /// <summary>
        /// Gets or sets the progamer identifier.
        /// </summary>
        /// <value>
        /// The progamer identifier.
        /// </value>
        protected string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the progamer.
        /// </summary>
        /// <value>
        /// The progamer.
        /// </value>
        protected Player Sender { get; set; }

        /// <summary>
        /// Gets the serialized progamer.
        /// </summary>
        /// <value>
        /// The serialized progamer.
        /// </value>
        protected string SerializedSender
        {
            get
            {
                return Sender != null
                    ? JsonConvert.SerializeObject(Sender)
                    : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the amater identifier.
        /// </summary>
        /// <value>
        /// The amater identifier.
        /// </value>
        protected string ReceiverId { get; set; }

        /// <summary>
        /// Gets or sets the amater.
        /// </summary>
        /// <value>
        /// The amater.
        /// </value>
        protected Player Receiver { get; set; }

        /// <summary>
        /// Gets the serialized amater.
        /// </summary>
        /// <value>
        /// The serialized amater.
        /// </value>
        protected string SerializedReceiver
        {
            get 
            { 
                return Receiver != null 
                    ? JsonConvert.SerializeObject(Receiver)
                    : string.Empty; 
            }
        }
        #endregion

        /// <summary>
        /// Initializes the helper properties.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected bool InitHelperProperties(HttpContext context)
        {
            var pc = new PlayerContext();
            var proc = new ProgamerContext();

            Sender = pc.GetPlayer(
                            context.User.Identity.GetUserId()
                        );
            ReceiverId = context.Request.QueryString["PlayerId"];
            Receiver = pc.GetPlayer(ReceiverId);
            if (Sender == null || Receiver == null) return false;
            var side = Sender.PlayerType;
            SenderId = Sender.PlayerId;
            if (side == PlayerTypesEnum.Amateur)
            {
                Receiver.Status = proc.GetProgamerStatus(ReceiverId);
            }
            else
            {
                Sender.Status = proc.GetProgamerStatus(SenderId);
            }
            return true;
        }

        /// <summary>
        /// Sets the status to players.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void SetStatusToPlayers(PlayerStatus status)
        {
            Sender.Status = status;
            Receiver.Status = status;
        }
    }
}