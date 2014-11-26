using PayPal.AdaptivePayments.Model;
using System.Collections.Generic;

namespace MushiDataTypes
{
    public class PayPalResponse
    {
        /// <summary>
        /// Gets or sets the ack.
        /// </summary>
        /// <value>
        /// The ack.
        /// </value>
        public AckCode? Ack { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>
        /// The redirect URL.
        /// </value>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the pay key.
        /// </summary>
        /// <value>
        /// The pay key.
        /// </value>
        public string PayKey { get; set; }

        /// <summary>
        /// Gets or sets the error data.
        /// </summary>
        /// <value>
        /// The error data.
        /// </value>
        public List<ErrorData> Errors{ get; set; }

        /// <summary>
        /// Gets or sets the warning data.
        /// </summary>
        /// <value>
        /// The warning data.
        /// </value>
        public List<WarningData> Warnings{ get; set; }
    }
}