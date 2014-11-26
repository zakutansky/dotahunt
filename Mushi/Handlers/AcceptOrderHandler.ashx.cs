using Mushi.Context;
using Mushi.Helper;
using MushiDataTypes.Enums;
using System.Web;

namespace Mushi.Handlers
{
    /// <summary>
    /// Summary description for AcceptOrderHandler
    /// </summary>
    public class AcceptOrderHandler : TransactionHelper, IHttpHandler
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
                //TODO internal error
                return;
            }
            SetStatusToPlayers(PlayerStatus.InvitationAccept);
            var order = goc.GetOrder(
                GameOrderStatesEnum.PendingInvitation,
                ReceiverId,
                SenderId);
            if (order == null)
            {
                //TODO order uz neexistuje, amater jom smazol
                var response = ClientHelper.SetResultForClient(false, SerializedReceiver);
                context.Response.Write(response);
                return;
            }
            var hasOrders = goc.ExistsOrdersInStates(
                SenderId,
                Sender.PlayerType,
                GameOrderStatesEnum.InvitationAccept,
                GameOrderStatesEnum.PendingPayment,
                GameOrderStatesEnum.InGame);
            if (hasOrders)
            {
                //TODO uz je pro ve hre, nebo cako na platbe insigo amatera, tak nimoze prijmout dalsi hre
                var response = ClientHelper.SetResultForClient(false, SerializedReceiver);
                context.Response.Write(response);
                return;
            }

            var success = goc.UpdateOrderState(order.OrderId, GameOrderStatesEnum.InvitationAccept);
            if (success)
            {
                //TODO ted zrovna moze dac amater Abort, doresit na klientovi
                HubContext.Instance.AcceptOrderAsync(SerializedSender, ReceiverId);
                var response = ClientHelper.SetResultForClient(true, SerializedReceiver);
                context.Response.Write(response);
            }
            else
            {
                //TODO amater zrusil objednavke
                var response = ClientHelper.SetResultForClient(false, SerializedReceiver);
                context.Response.Write(response);
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