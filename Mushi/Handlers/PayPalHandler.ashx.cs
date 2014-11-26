using System.Web;
using Mushi.Context;
using Mushi.Helper;
using MushiDataTypes.Enums;
using PayPal.AdaptivePayments.Model;

namespace Mushi.Handlers
{
    /// <summary>
    /// Summary description for PayPalHandler
    /// </summary>
    public class PayPalHandler : TransactionHelper, IHttpHandler
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
            SetStatusToPlayers(PlayerStatus.PendingPayment);
            var order = goc.GetOrder(
                GameOrderStatesEnum.InvitationAccept,
                SenderId,
                ReceiverId);
            if (order == null)
            {
                //TODO pro uz zrusil objednavke
                return;
            }
            var success = goc.UpdateOrderState(order.OrderId, GameOrderStatesEnum.PendingPayment);
            if (!success)
            {
                //TODO dodelat msg ze pro uz zrusil objednavku
                //TODO HubContext.Instance.AcceptOrderAsync(SerializedSender, ReceiverId);
                var response = ClientHelper.SetResultForClient(true, SerializedReceiver);
                context.Response.Write(response);
            }
            //TODO Odeslat zpravu pro ze nemuze rusit objednavku
            var payment = ppc.GetPayPalPayment(order.OrderId);
            if (payment != null)
            {
                ppc.DeletePayPalPayment(order.OrderId);
            }
            var playerEmail = Receiver.PayPalEmail;
            var playerAmount = order.OrderPrice;
            var shareAmount = ppc.CalculateShareFromAmount(playerAmount);
            var serverAuthority = context.Request.Url.Authority;
            //TODO kontrola na amount
            var result = ppc.DelayedChainedPayment(order.OrderId, ReceiverId, serverAuthority, playerEmail, playerAmount, shareAmount);
            if (result.Ack == AckCode.SUCCESS)
            {
                ppc.CreatePayPalPayment(order.OrderId, (int)result.Ack, result.PayKey, result.Status);
                var response = ClientHelper.SetResultForClient(true, result.RedirectUrl);
                context.Response.Write(response);
            }
            else
            {
                //TODO paypal neprosel, prozkoumac errory
                var response = ClientHelper.SetResultForClient(false, SerializedReceiver);
                context.Response.Write(response); 
            }
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