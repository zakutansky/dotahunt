using Mushi.Context;
using Mushi.Extensions;
using Mushi.Helper;
using MushiDataTypes.Enums;
using System.Web;
using Steam;

namespace Mushi.Handlers
{
    /// <summary>
    /// Summary description for CreateOrderHandler
    /// </summary>
    public class CreateOrderHandler : TransactionHelper, IHttpHandler
    {
        GameOrderContext goc = new GameOrderContext();

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            if (!InitHelperProperties(context))
            {
                //TODO doresit internal error
                return;
            }
            if (Receiver.Status != PlayerStatus.Online)
            {
                var result = ClientHelper.SetResultForClient(false, SerializedReceiver);
                context.Response.Write(result);
            }
            else
            {
                SetStatusToPlayers(PlayerStatus.PendingInvitation);
                var orderId = goc.TryCreateOrder(SenderId, ReceiverId, Receiver.Price);
                if (!string.IsNullOrEmpty(orderId))
                {
                    Sender.UpdateAvatar(SteamRequestsEnum.HireAPro);
                    HubContext.Instance.UpdatePlayerOrdersAsync(ReceiverId, SerializedSender);
                    var result = ClientHelper.SetResultForClient(true, SerializedReceiver);
                    context.Response.Write(result);
                }
                else
                {
                    //TODO uz existuje objednavka
                    var result = ClientHelper.SetResultForClient(false, SerializedReceiver);
                    context.Response.Write(result);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}