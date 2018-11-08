using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Data.Entities
{
    [Serializable]
    public class PlayerScore
    {
        public Player Player { get; private set; }
        public int Score { get; private set; }

        public PlayerScore(Player player, int score)
        {
            this.Player = player;
            this.Score = score;
        }
    }
}
