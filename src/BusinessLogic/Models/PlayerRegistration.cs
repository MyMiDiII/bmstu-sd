using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class PlayerRegistration
    {
        [Key]
        [ForeignKey("PlayerID")]
        public long PlayerID { get; set; }
        [Key]
        [ForeignKey("BoardGameEventID")]
        public long BoardGameEventID { get; set; }

        public virtual Player Player { get; set; }
        public virtual BoardGameEvent BoardGameEvent { get; set; }

        public PlayerRegistration(long playerID, long boardGameEventID)
        {
            PlayerID = playerID;
            BoardGameEventID = boardGameEventID;
        }
    }
}
