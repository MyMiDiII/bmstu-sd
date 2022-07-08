using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class PlayerRegistration
    {
        [Key]
        [ForeignKey("BoardGameEvent")]
        public long BoardGameEventID { get; set; }
        [Key]
        [ForeignKey("Player")]
        public long PlayerID { get; set; }

        public PlayerRegistration(long boardGameEventID, long playerID)
        {
            BoardGameEventID = boardGameEventID;
            PlayerID = playerID;
        }
    }
}
