using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class EventGame
    {
        [Key]
        [ForeignKey("BoardGame")]
        public long BoardGameID { get; set; }
        [Key]
        [ForeignKey("BoardGameEvent")]
        public long BoardGameEventID { get; set; }

        public EventGame(long boardGameID, long boardGameEventID)
        {
            BoardGameID = boardGameID;
            BoardGameEventID = boardGameEventID;
        }
    }
}
