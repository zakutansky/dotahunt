using System;
using System.Configuration;
using MushiDataTypes;
using MushiDb.Services;
using PayPal.AdaptivePayments;
using PayPal.AdaptivePayments.Model;
using System.Collections.Generic;

namespace Mushi.Context
{
    public class PayPalContext
    {
        /// <summary>
        /// Delayeds the chained payment.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="serverAuthority"></param>
        /// <param name="playerEmail">The player email.</param>
        /// <param name="playerAmount">The player amount.</param>
        /// <param name="shareAmount">The share amount.</param>
        /// <param name="oredrId"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public PayPalResponse DelayedChainedPayment(string oredrId,string playerId, string serverAuthority, string playerEmail, decimal playerAmount, decimal shareAmount)
        {
            var goc = new GameOrderContext();

            var receiverList = new ReceiverList
            {
                receiver = new List<Receiver>()
            };

            var request = new PayRequest();
            var requestEnvelope = new RequestEnvelope("en_US");
            request.requestEnvelope = requestEnvelope;
            request.actionType = "PAY_PRIMARY";
            request.feesPayer = "SECONDARYONLY";
            request.currencyCode = "USD";
            request.cancelUrl = @"http://" + serverAuthority + "/Sites/Mushi.aspx";
            request.returnUrl = @"http://" + serverAuthority + "/Handlers/PayPalReturnHandler.ashx?OrderId=" + oredrId + "&PlayerId=" + playerId;

            var player = new Receiver
            {
                email = playerEmail, 
                amount = playerAmount, 
                primary = true
            };
            receiverList.receiver.Add(player);



            var mushiApp = new Receiver
            {
                email = ConfigurationManager.AppSettings["PayPalAppEmail"],
                amount = shareAmount, 
                primary = false
            };
            receiverList.receiver.Add(mushiApp);

            var receiverlst = new ReceiverList(receiverList.receiver);
            request.receiverList = receiverlst;

            PayResponse response;
            try
            {
                var configurationMap = Configuration.GetAcctAndConfig();
                var service = new AdaptivePaymentsService(configurationMap);

                response = service.Pay(request);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var result = new PayPalResponse
            {
                Ack = response.responseEnvelope.ack,
                Errors = response.error
            };
            if (response.warningDataList != null) result.Warnings = response.warningDataList.warningData;
            if (result.Ack != null && result.Ack != AckCode.FAILURE && result.Ack != AckCode.FAILUREWITHWARNING)
            {
                result.RedirectUrl = ConfigurationManager.AppSettings["PaypalRedirectUrl"] + "_ap-payment&paykey=" + response.payKey;
                result.PayKey = response.payKey;
                result.Status = response.paymentExecStatus;
            }
            return result;
        }

        /// <summary>
        /// Gets the payment detail.
        /// </summary>
        /// <param name="paykey">The paykey.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public PayPalResponse GetPaymentDetail(string paykey)
        {
            var request = new PaymentDetailsRequest(new RequestEnvelope("en_US"))
            {
                payKey = paykey
            };

            PaymentDetailsResponse response = null;
            try
            {
                var configurationMap = Configuration.GetAcctAndConfig();
                var service = new AdaptivePaymentsService(configurationMap);
                response = service.PaymentDetails(request);
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
            var result = new PayPalResponse
            {
                Ack = response.responseEnvelope.ack,
                Errors = response.error
            };
            if (result.Ack != null && result.Ack != AckCode.FAILURE && result.Ack != AckCode.FAILUREWITHWARNING)
            {
                result.PayKey = paykey;
                result.Status = response.status;
            }
            return result;
        }

        /// <summary>
        /// Gets the share from amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public decimal CalculateShareFromAmount(decimal amount)
        {
            return (amount / 100) * Properties.Settings.Default.AmountShare;
        }

        /// <summary>
        /// Creates the pay pal payment.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="ack">The ack.</param>
        /// <param name="paykey">The paykey.</param>
        /// <param name="status">The status.</param>
        public void CreatePayPalPayment(string orderId, int ack, string paykey, string status)
        {
            using (var service = new HireProService())
            {
                service.CreatePayPalPayment(orderId, ack, paykey, status);
            }
        }

        /// <summary>
        /// Gets the pay pal payment.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        public MushiDb.PayPal GetPayPalPayment(string orderId)
        {
            using (var service = new HireProService())
            {
                return service.GetPayPalPayment(orderId);
            }
        }

        /// <summary>
        /// Deletes the pay pal payment.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        public void DeletePayPalPayment(string orderId)
        {
            using (var service = new HireProService())
            {
                service.DeletePayPalPayment(orderId);
            }
        }

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdatePaymentStatus(string orderId, string status)
        {
            using (var service = new HireProService())
            {
                service.UpdatePaymentStatus(orderId, status);
            }
        }
    }
}