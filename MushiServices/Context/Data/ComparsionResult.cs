using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushiServices.Context.Data
{
    public class ComparsionResult
    {
        /// <summary>
        /// Gets or sets the new players.
        /// </summary>
        /// <value>
        /// The new players.
        /// </value>
        public List<DotabuffPlayer> NewPlayers { get; set; }

        /// <summary>
        /// Gets or sets the old players.
        /// </summary>
        /// <value>
        /// The old players.
        /// </value>
        public List<ProgamerWithKey> OldPlayers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparsionResult"/> class.
        /// </summary>
        public ComparsionResult()
        {
            NewPlayers = new List<DotabuffPlayer>();
            OldPlayers = new List<ProgamerWithKey>();
        }
    }
}
