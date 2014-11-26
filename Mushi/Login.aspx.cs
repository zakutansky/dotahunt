using Mushi.Context;
using MushiDataTypes;
using MushiDataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mushi
{
    public partial class Login : Page
    {
        /// <summary>
        /// Gets or sets the type of the player.
        /// </summary>
        /// <value>
        /// The type of the player.
        /// </value>
        private PlayerTypesEnum PlayerType { get; set; }

        /// <summary>
        /// The lock object
        /// </summary>
        public static readonly object lockObject = new object();

        /// <summary>
        /// My key
        /// </summary>
        private const string progamersKey = "Progamers";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPlayersGrid();

                //await GetKick();
                //if (!string.IsNullOrEmpty(generalUserId)
                //    &&
                //    PlatformContextFactory.HasUserPlatformAccount(generalUserId,
                //        (int) Settings.Default.DefaultPlatform)
                //    &&
                //    PlayerContextFactory.HasAccountForCurrentGame(generalUserId,
                //        (int) Settings.Default.DefaultPlatform,
                //        Settings.Default.DefaultGame))
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvProPlayers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvProPlayers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btn = (Button)e.Row.FindControl("btnCreate");
                var player = (MushiDataTypes.Player)e.Row.DataItem;
                if (btn != null && player != null && player.Status == PlayerStatus.Offline)
                {
                    btn.Attributes.Add("disabled", "disabled");
                }
            }
        }

        /// <summary>
        /// Loads the players grid.
        /// </summary>
        private void LoadPlayersGrid()
        {
            List<Player> proPlayers;
            lock (lockObject)
            {
                proPlayers = Cache[progamersKey] as List<Player>;
                if (proPlayers == null)
                {
                    proPlayers = new ProgamerContext().GetProgamersAsc();
                    Cache.Add(progamersKey, proPlayers, null, DateTime.UtcNow.AddSeconds(86400), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            } 
            gvProPlayers.DataSource = proPlayers;
            gvProPlayers.DataBind();
        }
    }
}