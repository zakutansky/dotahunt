using Microsoft.AspNet.Identity;
using Mushi.Context;
using Mushi.Helper;
using MushiDataTypes;
using MushiDataTypes.Enums;
using Mushi.Properties;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Steam;
using Mushi.Extensions;

namespace Mushi.Sites
{
    public partial class Mushi : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        private string PlayerId { get; set; }

        /// <summary>
        /// Gets the pro player.
        /// </summary>
        /// <value>
        /// The pro player.
        /// </value>
        /// TODO prehodnotit jesli nevyjebac viewstate, zabiro o 1,5 kilo navic
        /// TODO TypeConverter skusic
        private Player ProPlayer
        {
            get
            {
                return ViewState["ProPlayer"] == null
                    ? null
                    : (Player)ViewState["ProPlayer"];
            }
            set
            {
                ViewState["ProPlayer"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the player.
        /// </summary>
        /// <value>
        /// The type of the player.
        /// </value>
        private PlayerTypesEnum PlayerType { get; set; }

        /// <summary>
        /// Gets the player context factory.
        /// </summary>
        /// <value>
        /// The player context factory.
        /// </value>
        private PlayerContext PlayerContextFactory
        {
            get { return new PlayerContext(); }
        }

        /// <summary>
        /// Gets the game order context context factory.
        /// </summary>
        /// <value>
        /// The game order context context factory.
        /// </value>
        private GameOrderContext GameOrderContextFactory
        {
            get { return new GameOrderContext(); }
        }

        /// <summary>
        /// Gets the progamer context factory.
        /// </summary>
        /// <value>
        /// The progamer context factory.
        /// </value>
        private ProgamerContext ProgamerContextFactory
        {
            get { return new ProgamerContext(); }
        }

        /// <summary>
        /// Gets the player auction context factory.
        /// </summary>
        /// <value>
        /// The player auction context factory.
        /// </value>
        private PlayerAuctionContext PlayerAuctionContextFactory
        {
            get { return new PlayerAuctionContext(); }
        }

        /// <summary>
        /// Gets the steam context factory.
        /// </summary>
        /// <value>
        /// The steam context factory.
        /// </value>
        private SteamContext SteamContextFactory
        {
            get { return new SteamContext(); }
        }
        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                if (userId == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                var player = PlayerContextFactory.GetPlayer(userId);
                if (player == null)
                {
                    Context.GetOwinContext().Authentication.SignOut();
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    PlayerId = player.PlayerId;
                    PlayerType = player.PlayerType;
                    if (PlayerType == PlayerTypesEnum.Pro && !string.IsNullOrEmpty(player.SteamId))
                    {
                        ProPlayer = player;
                        player.UpdateAvatar(SteamRequestsEnum.ProLoad);
                    }
                    var siteMaster = (SiteMaster)Master;
                    if (siteMaster != null) siteMaster.Player = player;
                }
                SetGui();
                LoadPlayersGrid();
            }
            //SetReadOnly();
            //ceCalendar.SelectedDate = DateTime.Now;
            //InitScheduler();
        }

        //private void SetReadOnly()
        //{
        //    tbDayPicker.Attributes.Add("readonly", "readonly");
        //}

        /// <summary>
        /// Sets the GUI.
        /// </summary>
        private void SetGui()
        {
            if (PlayerType == PlayerTypesEnum.Pro)
            {
                tabHireNow.Visible = false;
                tabHHireNow.Visible = false;
                cbAvailability.Checked = ProPlayer.Status == PlayerStatus.Online;
                tbPrice.Text = ProPlayer.Price.ToString();
            }
            else
            {
                tabHireList.Visible = false;
                tabHHireList.Visible = false;
            }
        }

        /// <summary>
        /// Loads the players grid.
        /// </summary>
        private void LoadPlayersGrid()
        {
            if (PlayerType == PlayerTypesEnum.Pro)
            {
                var proOrders = ProPlayer.Status == PlayerStatus.Online
                    ? GameOrderContextFactory.GetProOrdersByStates(
                        PlayerId,
                        GameOrderStatesEnum.PendingInvitation,
                        GameOrderStatesEnum.InvitationAccept,
                        GameOrderStatesEnum.PendingPayment,
                        GameOrderStatesEnum.InGame)
                    : GameOrderContextFactory.GetProOrdersByStates(
                        PlayerId,
                        GameOrderStatesEnum.InvitationAccept,
                        GameOrderStatesEnum.PendingPayment,
                        GameOrderStatesEnum.InGame);
                var amaterIds = proOrders.Select(o => o.BuyerId).ToArray();
                var amaters = PlayerContextFactory.GetAmatersByIds(amaterIds).ToList();
                PlayerContextFactory.SetStatusesToPlayersAccordingToOrders(amaters, proOrders);
                gvAmateurPlayers.DataSource = amaters;
                gvAmateurPlayers.DataBind();
            }
            //mozno nahradit procedurom
            var proPlayers = ProgamerContextFactory.GetProgamersAsc();
            var pendingOrders = GameOrderContextFactory.GetAmaterOrdersByStates(
                PlayerId,
                GameOrderStatesEnum.PendingInvitation,
                GameOrderStatesEnum.InvitationAccept,
                GameOrderStatesEnum.PendingPayment,
                GameOrderStatesEnum.InGame);
            PlayerContextFactory.SetStatusesToPlayersAccordingToOrders(proPlayers, pendingOrders);
            gvProPlayers.DataSource = proPlayers;
            gvProPlayers.DataBind();
        }

        /// <summary>
        /// Sets as available.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SetAsAvailable(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null) return;
            if (ProPlayer != null)
            {
                ProPlayer.Status = checkbox.Checked ? PlayerStatus.Online : PlayerStatus.Offline;
                if (ProPlayer.Status == PlayerStatus.Offline)
                {
                    DisconnectHelper.DoOnOffline(ProPlayer);
                }
                else
                {
                    ProgamerContextFactory.UpdateStatus(ProPlayer.PlayerId, PlayerStatus.Online);
                    HubContext.Instance.UpdatePlayer(JsonConvert.SerializeObject(ProPlayer));
                }
            }
            else
            {
                checkbox.Checked = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the SetPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SetPrice_Click(object sender, EventArgs e)
        {
            if (ProPlayer == null) return;
            decimal price;
            tbPrice.Text = tbPrice.Text.Replace(",", ".");
            if (!decimal.TryParse(tbPrice.Text, NumberStyles.Currency, CultureInfo.InvariantCulture, out price)) return;
            new ProgamerContext().UpdatePrice(ProPlayer.PlayerId, price);
            ProPlayer.Price = price;
            HubContext.Instance.UpdatePlayer(JsonConvert.SerializeObject(ProPlayer));
        }

        protected void gvProPlayers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btn = (Button)e.Row.FindControl("btnCreate");
                var player = (Player)e.Row.DataItem;
                if (btn != null && player != null && player.Status == PlayerStatus.Offline)
                {
                    btn.Attributes.Add("disabled", "disabled");
                }
            }
        }

        #region Auction
        /// <summary>
        /// Handles the Click event of the SetAuctionPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SetAuctionPrice_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(PlayerId))
            //{
            //    Application.Lock();
            //    decimal price;
            //    tbAuctionStartPrice.Text = tbAuctionStartPrice.Text.Replace(",", ".");
            //    if (decimal.TryParse(tbAuctionStartPrice.Text, NumberStyles.Currency, CultureInfo.InvariantCulture, out price))
            //    {
            //        var player = ProPlayers.Where(o => o.PlayerId == PlayerId).SingleOrDefault();
            //        if (player != null)
            //        {
            //            PlayerContextFactory.UpdatePlayerAuctionStartPrice(PlayerId, price);
            //            player.AuctionStartPrice = price;
            //        }
            //    }
            //    Application.UnLock();
            //}
        }

        /// <summary>
        /// Handles the RowCreated event of the gvAuctionScheudler control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvAuctionScheudler_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", "AuctionSchedulerRowClick('" + e.Row.RowIndex + "', this)");
            }
        }

        /// <summary>
        /// Handles the Click event of the btnConfirmAuction control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        //protected void btnConfirmAuction_Click(object sender, EventArgs e)
        //{
        //    if (ProPlayer != null)
        //    {
        //        Application.Lock();
        //        var price = ProPlayer.AuctionStartPrice;
        //        var playerId = ProPlayer.PlayerId;
        //        Application.UnLock();
        //        var selectedIndex = int.Parse(hfGvAuctionScheudlerSelectedRow.Value);
        //        var dataKey = gvAuctionScheudler.DataKeys[selectedIndex];
        //        if (dataKey != null)
        //        {
        //            var hours = (int)dataKey["Hour"];
        //            if (ScheduleDay != null && price != null)
        //            {
        //                var endDate = DateTimeHelper.SetHourForDate(ScheduleDay.Value, hours);
        //                var startDate = DateTime.Now;
        //                PlayerAuctionContextFactory.CreateNewAuction(startDate, endDate, price.Value, playerId);
        //            }
        //        }
        //    }
        //}
        
        //private void InitScheduler()
        //{
        //    gvAuctionScheudler.DataSource = PlayerAuctionContextFactory.InitAuctionHours();
        //    gvAuctionScheudler.DataBind();
        //}
        #endregion
    }
}