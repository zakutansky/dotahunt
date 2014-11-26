using Mushi.Context;
using Mushi.Helper;
using MushiDataTypes.Enums;
using System.Web;

namespace Mushi.Handlers
{
    /// <summary>
    /// Summary description for AbortOrderHandler
    /// </summary>
    public class AbortOrderHandler : TransactionHelper, IHttpHandler
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
            string buyerId, sellerId, result;
            if (Sender.PlayerType == PlayerTypesEnum.Amateur)
            {
                buyerId = SenderId;
                sellerId = ReceiverId;
            }
            else
            {
                buyerId = ReceiverId;
                sellerId = SenderId;
            }
            var order = goc.GetOrder(buyerId, sellerId);
            if (order == null)
            {
                //order does not exist
                result = ClientHelper.SetResultForClient(false, SerializedReceiver);
                context.Response.Write(result);
                return;
            }
            if (order.StateId == (int)GameOrderStatesEnum.PendingPayment)
            {
                //TODO nebeje mog nic zrobic v tyntu moment
            }
            if (order.StateId == (int) GameOrderStatesEnum.InvitationAccept)
            {
                goc.UpdateOrderState(order.OrderId, GameOrderStatesEnum.PaymentDeclined);
            }
            if (order.StateId == (int)GameOrderStatesEnum.InGame)
            {
                goc.UpdateOrderState(order.OrderId, GameOrderStatesEnum.Refund);
            }
            if (order.StateId == (int)GameOrderStatesEnum.PendingInvitation)
            {
                goc.RemoveOrder(order);
            }
            //TODO doresic na clientovi gdyz treba se pokusi smazac objednavke a uz je smazano
            HubContext.Instance.AbortOrderAsync(SerializedSender, ReceiverId);
            result = ClientHelper.SetResultForClient(true, SerializedReceiver);
            context.Response.Write(result);
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