using Mushi.Context;
using Mushi.Extensions;
using Mushi.Helper;
using MushiDataTypes.Enums;
using PayPal.AdaptivePayments.Model;
using System.Web;

namespace Mushi.Handlers
{
    /// <summary>
    /// Summary description for PayPalReturnHandler
    /// </summary>
    public class PayPalReturnHandler : TransactionHelper, IHttpHandler
    {
        GameOrderContext goc = new GameOrderContext();
        PayPalContext ppc = new PayPalContext();

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

            var orderId = context.Request.QueryString["OrderId"];
            if (string.IsNullOrEmpty(orderId))
            {
                //TODO posahany querystring z paypalu, zaznamenat resit osobne pak
            }

            var order = goc.GetOrder(SenderId, ReceiverId);
            if (orderId != order.OrderId && order.StateId != (int)GameOrderStatesEnum.PendingPayment)
            {
                //TODO stejne jak vyzi
            }

            var payment = ppc.GetPayPalPayment(orderId);
            if (payment == null)
            {
                //TODO chyba jak error
                return;
            }

            var payPalResult = ppc.GetPaymentDetail(payment.PayKey);
            if (payPalResult.Ack == AckCode.SUCCESS)
            {
                ppc.UpdatePaymentStatus(orderId, payPalResult.Status);
            }
            if (payPalResult.Status == "INCOMPLETE")
            {
                Sender.UpdateAvatar(SteamRequestsEnum.PreGame);
                Receiver.UpdateAvatar(SteamRequestsEnum.PreGame);
                SetStatusToPlayers(PlayerStatus.InGame);
                var roomId = GuidHelper.GetNoDashGuid();
                goc.UpdateOrderState(order.OrderId, GameOrderStatesEnum.InGame, roomId);
                Sender.RoomId = roomId;
                HubContext.Instance.OrderCompleteAsync(SerializedSender, ReceiverId);
            }
            else if (order.StateId == (int)GameOrderStatesEnum.PaymentDeclined &&
                     payPalResult.Status == "INCOMPLETE")
            {
                goc.UpdateOrderState(order.OrderId, GameOrderStatesEnum.Refund);
            }
            context.Server.Transfer("~Sites//Mushi.aspx");
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}