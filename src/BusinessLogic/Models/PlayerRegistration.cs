using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class PlayerRegistration
    {
        [Key]
        [ForeignKey("Player")]
        public long PlayerID { get; set; }
        [Key]
        [ForeignKey("BoardGameEvent")]
        public long BoardGameEventID { get; set; }

        public PlayerRegistration(long playerID, long boardGameEventID)
        {
            PlayerID = playerID;
            BoardGameEventID = boardGameEventID;
        }
    }
}
