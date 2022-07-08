using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class BGERegistration
    {
        [Key]
        [ForeignKey("BoardGameEvent")]
        public long BoardGameEventID { get; set; }
        [Key]
        [ForeignKey("Player")]
        public long PlayerID { get; set; }

        public BGERegistration(long boardGameEventID, long playerID)
        {
            BoardGameEventID = boardGameEventID;
            PlayerID = playerID;
        }
    }
}
