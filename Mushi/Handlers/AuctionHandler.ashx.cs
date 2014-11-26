using System.Web;
using System.Web.SessionState;
using Mushi.Context;
using Mushi.Helper;

namespace Mushi.Handlers
{
    /// <summary>
    /// Summary description for AuctionHandler
    /// </summary>
    public class AuctionHandler : IHttpHandler, IReadOnlySessionState
    {

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            var playerId = context.Session["PlayerId"] as string;
            var index = context.Request.QueryString["Index"];
            var date = context.Request.QueryString["Date"];
            if (!string.IsNullOrEmpty(playerId) && !string.IsNullOrEmpty(index) && !string.IsNullOrEmpty(date))
            {
                int hours;
                if (int.TryParse(index, out hours))
                {
                    var auctionDate = DateTimeHelper.GetDateFromString(date, Properties.Settings.Default.DateFormat);
                    if (auctionDate != null)
                    {
                        auctionDate = DateTimeHelper.SetHourForDate(auctionDate.Value, hours);
                        var canCreateAcution = new PlayerAuctionContext().CanCreateAuction(playerId, auctionDate.Value, Properties.Settings.Default.AuctionIntervalInHours);
                    }
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